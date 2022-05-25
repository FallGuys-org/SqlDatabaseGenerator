namespace FGO.Database.Models
{
    public class CustomisationItem
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }

        public Rarity? Rarity { get; set; }

        public CustomisationItemType? ItemType { get; set; }

        public List<ItemPrice> Prices { get; set; } = new();

        public List<CustomisationItemSource> Sources { get; set; } = new();

        public HashSet<string> Tags { get; set; } = new();
    }
}