namespace PokemonReviewApp.Models
{
    public class Reviewer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // one-to-MANY: A reviewer can have MANY reviews
        public ICollection<Review> Reviews { get; set; }
    }
}
