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
    }
}
