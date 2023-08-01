namespace PokemonReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // one-to-MANY: A country can have MANY owners
        public ICollection<Owner> Owners { get; set; }
    }
}
