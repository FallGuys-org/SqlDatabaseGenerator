namespace FGO.Database.Models
{
    public class Rarity
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }

        public int BackgroundColor { get; set; }

        public int ForegroundColor { get; set; }
    }
}