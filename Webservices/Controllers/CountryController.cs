using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Models;
using Webservices.Models.Repository;

namespace Webservices.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CountryController : ControllerBase
    {
        private readonly IDataRepository<Country> _dataRepository;
        public CountryController(IDataRepository<Country> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Country
        
        [HttpGet]
        public ActionResult<PagedCollectionResponse<Country>> Get([FromQuery] FilterModel filter)
        {
            IEnumerable<Country> country = _dataRepository.GetAll();
            //Filtering logic  
            Func<FilterModel, IEnumerable<Country>> filterData = (filterModel) =>
            {
                return country.Where(p => p.Name.StartsWith(filterModel.Term ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filter.Limit)
                .Take(filterModel.Limit);
            };

            //Get the data for the current page  
            var result = new PagedCollectionResponse<Country>();
            result.Items = filterData(filter);

            //Get next page URL string  
            FilterModel nextFilter = filter.Clone() as FilterModel;
            nextFilter.Page += 1;
            String nextUrl = filterData(nextFilter).Count() <= 0 ? null : this.Url.Action("Get", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            FilterModel previousFilter = filter.Clone() as FilterModel;
            previousFilter.Page -= 1;
            String previousUrl = previousFilter.Page <= 0 ? null : this.Url.Action("Get", null, previousFilter, Request.Scheme);

            result.NextPage = !String.IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !String.IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return result;
        }
        // GET: api/Country/5
        [HttpGet("{id}", Name = "GetCountry")]
        public IActionResult Get(long id)
        {
            Country country = _dataRepository.Get(id);

            if (country == null)
            {
                return NotFound("The country record couldn't be found.");
            }

            return Ok(country);
        }

        // POST: api/country
        [HttpPost]
        public IActionResult Post([FromBody] Country country)
        {
            if (country == null)
            {
                return BadRequest("country is null.");
            }

            _dataRepository.Add(country);
            return CreatedAtRoute(
                  "GetCountry",
                  new { Id = country.ID },
                  country);
        }

        // PUT: api/Country/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Country country)
        {
            if (country == null)
            {
                return BadRequest("country is null.");
            }

            Country countryToUpdate = _dataRepository.Get(id);
            if (countryToUpdate == null)
            {
                return NotFound("The country record couldn't be found.");
            }

            _dataRepository.Update(countryToUpdate, country);
            return NoContent();
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Country country = _dataRepository.Get(id);
            if (country == null)
            {
                return NotFound("The country record couldn't be found.");
            }

            _dataRepository.Delete(country);
            return NoContent();
        }

    }
}