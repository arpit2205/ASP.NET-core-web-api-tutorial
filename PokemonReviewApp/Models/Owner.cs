namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gym { get; set; }

        // ONE-to-many: An owner will have ONE country
        public Country Country { get; set; }

        // many-to-many
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }
}
