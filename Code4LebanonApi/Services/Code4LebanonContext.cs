using Microsoft.EntityFrameworkCore;

namespace Code4LebanonApi.Services
{
    public class Code4LebanonContext : DbContext
    {
        public Code4LebanonContext(DbContextOptions<Code4LebanonContext> options) : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
    }
}
