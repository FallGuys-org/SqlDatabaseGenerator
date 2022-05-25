namespace FGO.Database.Models
{
    public class CustomisationItem
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }

        public Rarity? Rarity { get; set; }

        public CustomisationItemType? ItemType { get; set; }

        public ItemPrice[] Prices { get; set; } = Array.Empty<ItemPrice>();

        public CustomisationItemSource[] Sources { get; set; } = Array.Empty<CustomisationItemSource>();

        public Tag[] Tags { get; set; } = Array.Empty<Tag>();
    }
}