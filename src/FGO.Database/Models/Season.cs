namespace FGO.Database.Models
{
    public class Season
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }

        public LocalisedString? Theme { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}