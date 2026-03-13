using HRMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Api
{
    public static class MingrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<HRMSContext>())
                {
                    context.Database.Migrate();
                    new DataSeeder().SeedAsync(context).Wait();
                }
            }
            return app;
        }
    }
}
