using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrickFactoryBeat.Infrastructure.Persistence
{
    /// <summary>
    /// Just for creating the initial migration
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=LegoEquipmentDb;Trusted_Connection=True;";
            // Use your actual connection string
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}