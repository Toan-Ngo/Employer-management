using HRMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Api
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<HRMSContext>();
                var seeder = services.GetRequiredService<DataSeeder>();

                context.Database.Migrate();

                seeder.SeedAsync().Wait();
            }

            return app;
        }
    }
}