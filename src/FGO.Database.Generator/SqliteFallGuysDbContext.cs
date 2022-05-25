using Microsoft.EntityFrameworkCore;

namespace FGO.Database.Generator
{
    public class SqliteFallGuysDbContext : FallGuysDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=fallguys.db");
        }
    }
}