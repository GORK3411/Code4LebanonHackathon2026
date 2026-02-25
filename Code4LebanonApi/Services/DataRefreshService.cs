using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Code4LebanonApi.Models;

// NOTE: This implementation adds lightweight retry/backoff (Polly-like) logic
// and persistent checkpointing via the SyncStatus table. It fetches pages
// from `NumuSurveyService` and commits each page before fetching the next one
// which keeps memory bounded and makes the refresh resumable.

namespace Code4LebanonApi.Services
{
    // Background service that refreshes data from the Numu survey API every 15 minutes.
    public class DataRefreshService : BackgroundService
    {
        private readonly ILogger<DataRefreshService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly NumuSurveyService _numu;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);
        private readonly int _pageLimit = 200;
        private readonly int _maxRetries = 3;
        private readonly TimeSpan _initialBackoff = TimeSpan.FromSeconds(2);

        public DataRefreshService(ILogger<DataRefreshService> logger, IServiceScopeFactory scopeFactory, NumuSurveyService numu)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _numu = numu;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DataRefreshService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RefreshOnce(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during data refresh");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("DataRefreshService stopped.");
        }
        private async Task RefreshOnce(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Code4LebanonContext>();


            // Load or create checkpoint
            var syncName = "NumuResponses";
            var status = context.SyncStatuses.FirstOrDefault(s => s.ServiceName == syncName);
            if (status == null)
            {
                status = new SyncStatus { ServiceName = syncName };
                context.SyncStatuses.Add(status);
                await context.SaveChangesAsync(ct);
            }

            var start = status.LastProcessedAt.HasValue ? status.LastProcessedAt.Value.AddSeconds(1) : DateTime.UtcNow.AddDays(-30);
            var end = DateTime.UtcNow;

            _logger.LogInformation("Refreshing responses from {start} to {end} (checkpoint {checkpoint})", start, end, status.LastProcessedAt);

            int page = 1;
            bool more = true;
            int imported = 0;

            while (more && !ct.IsCancellationRequested)
            {
                // Fetch a single page with retry/backoff
                List<SurveyResponse> responses = null;
                try
                {
                    responses = await ExecuteWithRetryAsync(() => _numu.GetResponsesAsync(start, end, page: page, limit: _pageLimit), _maxRetries, _initialBackoff, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to fetch page {page} after retries; aborting this run", page);
                    break;
                }

                if (responses == null || responses.Count == 0)
                {
                    more = false;
                    break;
                }

                foreach (var r in responses)
                {
                    try
                    {
                        var existing = await context.SurveyResponses.FindAsync(new object[] { r.Id }, ct);

                        // Extract parsed fields from r.Responses (dictionary) defensively
                        string areas = null;
                        string motivation = null;
                        string challenges = null;

                        if (r.Responses != null)
                        {
                            if (r.Responses.TryGetValue("areas_of_interest", out var a)) areas = a?.ToString();
                            if (r.Responses.TryGetValue("areasOfInterest", out var a2) && string.IsNullOrEmpty(areas)) areas = a2?.ToString();
                            if (r.Responses.TryGetValue("motivation", out var m)) motivation = m?.ToString();
                            if (r.Responses.TryGetValue("motives", out var m2) && string.IsNullOrEmpty(motivation)) motivation = m2?.ToString();
                            if (r.Responses.TryGetValue("challenges", out var c)) challenges = c?.ToString();
                        }

                        if (existing == null)
                        {
                            var newEntity = new SurveyResponse
                            {
                                Id = r.Id,
                                SurveyId = r.SurveyId,
                                RespondentEmail = r.RespondentEmail,
                                RespondentPhone = r.RespondentPhone,
                                RespondentName = r.RespondentName,
                                UtmSource = r.UtmSource,
                                UtmMedium = r.UtmMedium,
                                GeoCountry = r.GeoCountry,
                                GeoRegion = r.GeoRegion,
                                GeoCity = r.GeoCity,
                                CreatedAt = r.CreatedAt,
                                AreasOfInterest = areas,
                                Motivation = motivation,
                                Challenges = challenges
                            };

                            await context.SurveyResponses.AddAsync(newEntity, ct);
                            imported++;
                        }
                        else
                        {
                            existing.UtmSource = r.UtmSource;
                            existing.GeoCountry = r.GeoCountry;
                            existing.GeoRegion = r.GeoRegion;
                            existing.GeoCity = r.GeoCity;
                            existing.CreatedAt = r.CreatedAt;
                            existing.AreasOfInterest = areas ?? existing.AreasOfInterest;
                            existing.Motivation = motivation ?? existing.Motivation;
                            existing.Challenges = challenges ?? existing.Challenges;

                            context.SurveyResponses.Update(existing);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to upsert response {id}", r.Id);
                    }
                }

                // Commit each page to keep memory bounded and to make progress durable
                await context.SaveChangesAsync(ct);

                // Update checkpoint after successful commit
                var maxCreated = responses.Max(x => x.CreatedAt);
                var lastId = responses.Last().Id;
                status.LastProcessedAt = maxCreated > (status.LastProcessedAt ?? DateTime.MinValue) ? maxCreated : status.LastProcessedAt;
                status.LastProcessedId = lastId;
                status.LastRunAt = DateTime.UtcNow;
                context.SyncStatuses.Update(status);
                await context.SaveChangesAsync(ct);

                // If we received less than pageLimit, no more pages
                if (responses.Count < _pageLimit) more = false;
                else page++;
            }

            _logger.LogInformation("Data refresh complete. Imported: {imported}. LastProcessedAt={last}", imported, status.LastProcessedAt);
        }

        // Lightweight retry with exponential backoff + jitter (Polly-like)
        private static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int maxRetries, TimeSpan initialDelay, CancellationToken ct)
        {
            var delay = initialDelay;
            var rand = new Random();

            for (int attempt = 1; ; attempt++)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    return await action();
                }
                catch (Exception) when (attempt <= maxRetries)
                {
                    // jittered backoff
                    var jitterMs = rand.Next(0, 300);
                    await Task.Delay(delay + TimeSpan.FromMilliseconds(jitterMs), ct);
                    delay = TimeSpan.FromMilliseconds(Math.Min(delay.TotalMilliseconds * 2, 30000));
                }
            }
        }
}

}
