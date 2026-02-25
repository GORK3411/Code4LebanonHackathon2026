using System;
using System.Collections.Generic;
using System.Linq;

namespace Code4LebanonApi.Services
{
    public static class DbSeeder
    {
        public static void Seed(Code4LebanonContext context)
        {
            // Ensure database/table creation for in-memory or provider
            context.Database.EnsureCreated();

            if (context.Surveys.Any()) return; // already seeded

            var now = DateTime.UtcNow;

            var survey1 = new Survey
            {
                Id = Guid.NewGuid().ToString(),
                Slug = "community-needs-2026",
                Title = "Community Needs Survey 2026",
                Description = "Short survey about community needs",
                Schema = new SurveySchema
                {
                    Sections = new List<SurveySection>
                    {
                        new SurveySection { Id = "s1", Title = "General" },
                        new SurveySection { Id = "s2", Title = "Services" }
                    }
                },
                IsActive = true,
                AllowMultipleSubmissions = false,
                RequiresAuth = false,
                CreatedAt = now.AddDays(-10),
                UpdatedAt = now.AddDays(-5),
                PublishedAt = now.AddDays(-9),
                ExpiresAt = now.AddDays(30)
            };

            var survey2 = new Survey
            {
                Id = Guid.NewGuid().ToString(),
                Slug = "closed-survey",
                Title = "Closed Survey",
                Description = "An expired survey",
                Schema = new SurveySchema
                {
                    Sections = new List<SurveySection>
                    {
                        new SurveySection { Id = "s1", Title = "Feedback" }
                    }
                },
                IsActive = false,
                AllowMultipleSubmissions = true,
                RequiresAuth = false,
                CreatedAt = now.AddMonths(-2),
                UpdatedAt = now.AddMonths(-1),
                PublishedAt = now.AddMonths(-2),
                ExpiresAt = now.AddMonths(-1)
            };

            context.Surveys.AddRange(survey1, survey2);

            // Add some responses for survey1 with different regions
            var responses = new List<SurveyResponse>
            {
                new SurveyResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    SurveyId = survey1.Id,
                    RespondentEmail = "alice@example.com",
                    RespondentPhone = "+96170000001",
                    RespondentName = "Alice",
                    Responses = new Dictionary<string, object> { { "q1", "yes" }, { "q2", 5 } },
                    UtmSource = "facebook",
                    UtmMedium = "social",
                    GeoCountry = "Lebanon",
                    GeoRegion = "Beirut",
                    GeoCity = "Beirut",
                    CreatedAt = now.AddDays(-8)
                },
                new SurveyResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    SurveyId = survey1.Id,
                    RespondentEmail = "bob@example.com",
                    RespondentPhone = "+96170000002",
                    RespondentName = "Bob",
                    Responses = new Dictionary<string, object> { { "q1", "no" }, { "q2", 3 } },
                    UtmSource = "newsletter",
                    UtmMedium = "email",
                    GeoCountry = "Lebanon",
                    GeoRegion = "Mount Lebanon",
                    GeoCity = "Jounieh",
                    CreatedAt = now.AddDays(-7)
                },
                new SurveyResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    SurveyId = survey1.Id,
                    RespondentEmail = "carol@example.com",
                    RespondentPhone = "+96170000003",
                    RespondentName = "Carol",
                    Responses = new Dictionary<string, object> { { "q1", "yes" }, { "q2", 4 } },
                    UtmSource = "twitter",
                    UtmMedium = "social",
                    GeoCountry = "Lebanon",
                    GeoRegion = "Beirut",
                    GeoCity = "Beirut",
                    CreatedAt = now.AddDays(-6)
                }
            };

            context.SurveyResponses.AddRange(responses);

            context.SaveChanges();
        }
    }
}
