﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public Owner GetOwner(int id)
        {
            return _context.Owners.Where(o => o.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public bool OwnerExists(int id)
        {
            return _context.Owners.Any(o => o.Id == id);
        }

        public ICollection<Owner> GetOwnerByPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(po => po.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(po => po.Owner.Id == ownerId).Select(po => po.Pokemon).ToList();
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
