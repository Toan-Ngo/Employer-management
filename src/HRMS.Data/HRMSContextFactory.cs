using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HRMS.Data
{
    public class HRMSContextFactory : IDesignTimeDbContextFactory<HRMSContext>
    {
        public HRMSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRMSContext>();

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new HRMSContext(optionsBuilder.Options);
        }
    }
}