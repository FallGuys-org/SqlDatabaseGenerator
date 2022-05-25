using FGO.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FGO.Database
{
    public class FallGuysDbContext : DbContext
    {
        public DbSet<Rarity> Rarities { get; set; } = null!;

        public DbSet<Currency> Currencies { get; set; } = null!;

        public DbSet<CustomisationItemType> CustomisationItemTypes { get; set; } = null!;

        public DbSet<CustomisationItemSource> CustomisationItemSources { get; set;} = null!;

        public DbSet<CustomisationItem> CustomisationItems { get; set; } = null!;

        public DbSet<Season> Seasons { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomisationItem>()
                .OwnsMany(c => c.Prices);
        }
    }
}