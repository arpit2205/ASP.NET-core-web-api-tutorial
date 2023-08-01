namespace PokemonReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        // ONE-to-many: A review will have ONE reviewer and ONE pokemon
        public Reviewer Reviewer { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
