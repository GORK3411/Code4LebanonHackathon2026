using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Code4LebanonApi.Services
{
    // Background service that refreshes data from the Numu survey API every 15 minutes.
    public class DataRefreshService : BackgroundService
    {
        private readonly ILogger<DataRefreshService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly NumuSurveyService _numu;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

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

            // Determine last saved response date to fetch only new entries
            var last = context.SurveyResponses.OrderByDescending(r => r.CreatedAt).Select(r => r.CreatedAt).FirstOrDefault();
            var start = last == default ? DateTime.UtcNow.AddDays(-30) : last.AddSeconds(1);
            var end = DateTime.UtcNow;

            _logger.LogInformation("Refreshing responses from {start} to {end}", start, end);

            int page = 1;
            const int pageLimit = 100;
            bool more = true;
            int imported = 0;

            while (more && !ct.IsCancellationRequested)
            {
                var responses = await _numu.GetResponsesAsync(start, end, page: page, limit: pageLimit);
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
                            // New record
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
                            // Update some fields
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

                await context.SaveChangesAsync(ct);

                // If we received less than pageLimit, no more pages
                if (responses.Count < pageLimit) more = false;
                else page++;
            }

            // Cleanup: remove responses older than 2 years to avoid db growth (adjustable)
            try
            {
                var cutoff = DateTime.UtcNow.AddYears(-2);
                var old = context.SurveyResponses.Where(r => r.CreatedAt < cutoff).ToList();
                if (old.Count > 0)
                {
                    context.SurveyResponses.RemoveRange(old);
                    await context.SaveChangesAsync(ct);
                    _logger.LogInformation("Removed {count} old responses older than {cutoff}", old.Count, cutoff);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to purge old records");
            }

            _logger.LogInformation("Data refresh complete. Imported: {imported}", imported);
        }
    }
}
