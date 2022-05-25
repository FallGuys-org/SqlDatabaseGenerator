using FGO.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            modelBuilder.Entity<CustomisationItem>()
                .HasMany(c => c.Sources)
                .WithMany(c => c.Items);

            // Tags are serialized as a semicolon delimited string
            var splitStringConverter = new ValueConverter<HashSet<string>, string>(v => string.Join(";", v), v => new HashSet<string>(v.Split(new[] { ';' })));
            modelBuilder.Entity<CustomisationItem>()
                .Property(c => c.Tags)
                .HasConversion(splitStringConverter);
        }
    }
}