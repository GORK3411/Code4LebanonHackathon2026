
using Code4LebanonApi.Services;
using Microsoft.EntityFrameworkCore;
namespace Code4LebanonApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<NumuSurveyService>();
            // Background data refresher that pulls data from the source API every 15 minutes
            builder.Services.AddHostedService<Code4LebanonApi.Services.DataRefreshService>();

            // Register EF Core DbContext for Code4Lebanon database.
            var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
            if (useInMemory)
            {
                builder.Services.AddDbContext<Code4LebanonContext>(options =>
                    options.UseInMemoryDatabase("Code4LebanonSim"));
            }
            else
            {
                // Ensure you have the package Microsoft.EntityFrameworkCore.SqlServer installed and
                // set connection string named "Code4Lebanon" in appsettings.json.
                builder.Services.AddDbContext<Code4LebanonContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Code4Lebanon")));
            }

            builder.Services.AddScoped<Code4LebanonRepository>();
            var app = builder.Build();

            // Seed database with sample "fodder" data when using in-memory DB
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Code4LebanonContext>();
                    DbSeeder.Seed(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
