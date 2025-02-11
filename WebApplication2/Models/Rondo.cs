namespace WebApplication2.Models
{
    public class Rondo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Country>? VisitedCountries { get; set; }
    }
}
