namespace FGO.Database.Models
{
    public class CustomisationItemSource
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }

        public List<CustomisationItem> Items { get; set; } = new();
    }
}