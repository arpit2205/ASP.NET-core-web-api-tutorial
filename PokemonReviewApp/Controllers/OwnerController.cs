using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Owner))]
        public IActionResult GetOwner(int id)
        {
            if(!_ownerRepository.OwnerExists(id))
            {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if(!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto owner)
        {
            if(owner == null)
            {
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(owner);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if(!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Error saving model");
                StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

    }
}
