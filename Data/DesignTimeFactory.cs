using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Server.Data
{
    public class ServerDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost, 1434;Database=ServerDb;User ID=sa;Password=1234567,dG;TrustServerCertificate=True;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}