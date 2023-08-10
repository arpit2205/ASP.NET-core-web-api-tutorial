using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Owner GetOwner(int id);

        ICollection<Owner> GetOwners();

        ICollection<Owner> GetOwnerByPokemon(int pokeId);

        ICollection<Pokemon> GetPokemonByOwner(int ownerId);

        bool OwnerExists(int id);

        bool CreateOwner(Owner owner);

        bool Save();
    }
}
