using Microsoft.EntityFrameworkCore;

namespace FGO.Database.Generator
{
    public class SqliteFallGuysDbContext : FallGuysDbContext
    {
        public SqliteFallGuysDbContext(Uri sqliteDatabasePath)
        {
            SqliteDatabasePath = sqliteDatabasePath;
        }

        public Uri SqliteDatabasePath { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={SqliteDatabasePath}");
        }
    }
}