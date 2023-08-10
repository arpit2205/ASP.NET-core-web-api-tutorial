using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type =typeof(Country))]
        public IActionResult GetCountry(int id)
        {
            if(!_countryRepository.CountryExists(id))
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(id));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {

            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateCountry([FromBody] CountryDto country)
        {
            if(country == null)
            {
                return BadRequest(ModelState);
            }

            var countryAlreadyExists = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == country.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(countryAlreadyExists != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryMap = _mapper.Map<Country>(country);

            if(!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Error saving the category");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created");
        }



    }
}
