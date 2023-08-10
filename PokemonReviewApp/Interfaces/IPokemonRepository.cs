using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);

        Pokemon GetPokemon(string name);

        bool PokemonExists(int id);

        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool Save();
    }
}
