namespace FGO.Database.Models
{
    public class Tag
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Label { get; set; }
    }
}