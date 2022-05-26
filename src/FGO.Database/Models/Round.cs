namespace FGO.Database.Models
{
    public class Round
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }

        public Season? Season { get; set; }

        public RoundType? RoundType { get; set; }

        public LocalisedString? Objective { get; set; }

        public LocalisedString? HowTo { get; set; }
    }
}