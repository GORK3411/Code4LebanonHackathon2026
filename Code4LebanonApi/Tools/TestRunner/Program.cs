using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Code4LebanonApi.Services;

class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("Running simple test runner...");
        try
        {
            using var ctx = CreateInMemoryContext("testrunner");
            ctx.SurveyResponses.AddRange(new[] {
                new SurveyResponse { Id = "1", UtmSource = "University of Beirut", CreatedAt = DateTime.UtcNow },
                new SurveyResponse { Id = "2", UtmSource = "Some NGO", CreatedAt = DateTime.UtcNow },
                new SurveyResponse { Id = "3", AreasOfInterest = "GenAI,AI Fundamentals", CreatedAt = DateTime.UtcNow }
            });
            ctx.SaveChanges();

            var repo = new Code4LebanonRepository(ctx);
            var channels = repo.GetRegistrationsByChannelAsync().GetAwaiter().GetResult();
            var areas = repo.GetAreasOfInterestAsync().GetAwaiter().GetResult();

            if (!channels.ContainsKey("Universities") || channels["Universities"] < 1)
                throw new Exception("Registrations by channel test failed");
            if (!areas.ContainsKey("GenAI"))
                throw new Exception("Areas of interest test failed");

            Console.WriteLine("All tests passed.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Test failed: " + ex.Message);
            return 2;
        }
    }

    static Code4LebanonContext CreateInMemoryContext(string name)
    {
        var options = new DbContextOptionsBuilder<Code4LebanonContext>()
            .UseInMemoryDatabase(name)
            .Options;
        return new Code4LebanonContext(options);
    }
}
