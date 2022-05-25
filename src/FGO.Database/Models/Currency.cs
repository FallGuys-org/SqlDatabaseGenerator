namespace FGO.Database.Models
{
    public class Currency
    {
        public string Id { get; set; } = null!;

        public LocalisedString? Name { get; set; }
    }
}