using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Code4LebanonApi.Services
{
    public class Code4LebanonRepository
    {
        private readonly Code4LebanonContext _context;

        public class TimeSeriesPoint
        {
            public string Period { get; set; }
            public int Count { get; set; }
        }

        public class ChannelRegionCount
        {
            public string Channel { get; set; }
            public string Region { get; set; }
            public int Count { get; set; }
        }

        public Code4LebanonRepository(Code4LebanonContext context)
        {
            _context = context;
        }

        // Number of applicants per region (group by geo_region)
        public async Task<Dictionary<string, int>> GetApplicantsPerRegionAsync()
        {
            return await _context.SurveyResponses
                .AsNoTracking()
                .GroupBy(r => string.IsNullOrEmpty(r.GeoRegion) ? "Unknown" : r.GeoRegion)
                .Select(g => new { Region = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Region, x => x.Count);
        }

        // All published surveys
        public async Task<List<Survey>> GetPublishedSurveysAsync()
        {
            return await _context.Surveys
                .AsNoTracking()
                .Where(s => s.PublishedAt != null)
                .ToListAsync();
        }

        // All published AND active surveys (based on expiry)
        public async Task<List<Survey>> GetActivePublishedSurveysAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Surveys
                .AsNoTracking()
                .Where(s => s.PublishedAt != null && (s.ExpiresAt == null || s.ExpiresAt > now))
                .ToListAsync();
        }

        // All user responses for a specific survey id
        public async Task<List<SurveyResponse>> GetResponsesForSurveyAsync(string surveyId)
        {
            return await _context.SurveyResponses
                .AsNoTracking()
                .Where(r => r.SurveyId == surveyId)
                .ToListAsync();
        }

        // A: Registrations by channel (Universities, Syndicates, Public Sector, NGOs, Employers, Other)
        public async Task<Dictionary<string,int>> GetRegistrationsByChannelAsync()
        {
            var groups = await _context.SurveyResponses
                .AsNoTracking()
                .GroupBy(r => r.UtmSource ?? "Unknown")
                .Select(g => new { Channel = g.Key, Count = g.Count() })
                .ToListAsync();

            // Map known channel keywords into buckets
            var buckets = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Universities", 0 },
                { "Syndicates", 0 },
                { "Public Sector", 0 },
                { "NGOs", 0 },
                { "Employers", 0 },
                { "Other", 0 }
            };

            foreach (var g in groups)
            {
                var key = (g.Channel ?? string.Empty).ToLowerInvariant();
                if (key.Contains("university") || key.Contains("uni") ) buckets["Universities"] += g.Count;
                else if (key.Contains("syndicat") || key.Contains("syndicate")) buckets["Syndicates"] += g.Count;
                else if (key.Contains("ngo")) buckets["NGOs"] += g.Count;
                else if (key.Contains("employ") || key.Contains("company") || key.Contains("employer")) buckets["Employers"] += g.Count;
                else if (key.Contains("public") || key.Contains("government") || key.Contains("ministry")) buckets["Public Sector"] += g.Count;
                else buckets["Other"] += g.Count;
            }

            return buckets;
        }

        // A-3 Growth: registrations over time (daily)
        public async Task<List<TimeSeriesPoint>> GetRegistrationsGrowthAsync(DateTime from, DateTime to)
        {
            var data = await _context.SurveyResponses
                .AsNoTracking()
                .Where(r => r.CreatedAt >= from && r.CreatedAt <= to)
                .GroupBy(r => r.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            // Fill full range (daily)
            var result = new List<TimeSeriesPoint>();
            for (var d = from.Date; d <= to.Date; d = d.AddDays(1))
            {
                var found = data.FirstOrDefault(x => x.Date == d);
                result.Add(new TimeSeriesPoint { Period = d.ToString("yyyy-MM-dd"), Count = found?.Count ?? 0 });
            }

            return result;
        }

        // A-2 Deep dive: breakdown by specific utm_source values (top N)
        public async Task<Dictionary<string,int>> GetDeepDiveByEntityAsync(string contains, int top = 20)
        {
            if (string.IsNullOrEmpty(contains)) contains = string.Empty;
            contains = contains.ToLowerInvariant();

            var groups = await _context.SurveyResponses
                .AsNoTracking()
                .Where(r => (r.UtmSource ?? string.Empty).ToLower().Contains(contains))
                .GroupBy(r => r.UtmSource ?? "Unknown")
                .Select(g => new { Channel = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .ToListAsync();

            return groups.ToDictionary(x => x.Channel, x => x.Count);
        }

        // B-1 Areas of Interest (parse comma-separated AreasOfInterest)
        public async Task<Dictionary<string,int>> GetAreasOfInterestAsync()
        {
            var rows = await _context.SurveyResponses
                .AsNoTracking()
                .Where(r => !string.IsNullOrEmpty(r.AreasOfInterest))
                .Select(r => r.AreasOfInterest)
                .ToListAsync();

            var counts = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in rows)
            {
                var parts = row.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim());
                foreach (var p in parts)
                {
                    if (string.IsNullOrEmpty(p)) continue;
                    if (!counts.ContainsKey(p)) counts[p] = 0;
                    counts[p]++;
                }
            }

            return counts.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        // B-2 Motivations
        public async Task<Dictionary<string,int>> GetMotivationsAsync()
        {
            var groups = await _context.SurveyResponses
                .AsNoTracking()
                .GroupBy(r => string.IsNullOrEmpty(r.Motivation) ? "Unknown" : r.Motivation)
                .Select(g => new { Motivation = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return groups.ToDictionary(x => x.Motivation, x => x.Count);
        }

        // B-3 Challenges
        public async Task<Dictionary<string,int>> GetChallengesAsync()
        {
            var rows = await _context.SurveyResponses
                .AsNoTracking()
                .Where(r => !string.IsNullOrEmpty(r.Challenges))
                .Select(r => r.Challenges)
                .ToListAsync();

            var counts = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in rows)
            {
                var parts = row.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim());
                foreach (var p in parts)
                {
                    if (string.IsNullOrEmpty(p)) continue;
                    if (!counts.ContainsKey(p)) counts[p] = 0;
                    counts[p]++;
                }
            }

            return counts.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        // C-1 Channel effectiveness by region
        public async Task<List<ChannelRegionCount>> GetChannelEffectivenessByRegionAsync()
        {
            var data = await _context.SurveyResponses
                .AsNoTracking()
                .GroupBy(r => new { Channel = r.UtmSource ?? "Unknown", Region = string.IsNullOrEmpty(r.GeoRegion) ? "Unknown" : r.GeoRegion })
                .Select(g => new ChannelRegionCount { Channel = g.Key.Channel, Region = g.Key.Region, Count = g.Count() })
                .ToListAsync();

            return data.OrderByDescending(d => d.Count).ToList();
        }

        // C-2 Gap analysis: return counts per region and mark low-performing regions
        public async Task<Dictionary<string,int>> GetRegionDistributionAsync()
        {
            var groups = await _context.SurveyResponses
                .AsNoTracking()
                .GroupBy(r => string.IsNullOrEmpty(r.GeoRegion) ? "Unknown" : r.GeoRegion)
                .Select(g => new { Region = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return groups.ToDictionary(x => x.Region, x => x.Count);
        }
    }
}
