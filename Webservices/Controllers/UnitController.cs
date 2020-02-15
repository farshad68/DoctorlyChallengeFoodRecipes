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
    public class UnitController : ControllerBase
    {
        private readonly IDataRepository<Unit> _dataRepository;
        public UnitController(IDataRepository<Unit> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Unit
        [HttpGet]
        public ActionResult<PagedCollectionResponse<Unit>> Get([FromQuery] FilterModel filter)
        {
            IEnumerable<Unit> unit = _dataRepository.GetAll();
            //Filtering logic  
            Func<FilterModel, IEnumerable<Unit>> filterData = (filterModel) =>
            {
                return unit.Where(p => p.Name.StartsWith(filterModel.Term ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filter.Limit)
                .Take(filterModel.Limit);
            };

            //Get the data for the current page  
            var result = new PagedCollectionResponse<Unit>();
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
        // GET: api/Unit/5
        [HttpGet("{id}", Name = "GetUnit")]
        public IActionResult Get(long id)
        {
            Unit unit = _dataRepository.Get(id);

            if (unit == null)
            {
                return NotFound("The unit record couldn't be found.");
            }

            return Ok(unit);
        }

        // POST: api/Unit
        [HttpPost]
        public IActionResult Post([FromBody] Unit unit)
        {
            if (unit == null)
            {
                return BadRequest("unit is null.");
            }

            _dataRepository.Add(unit);
            return CreatedAtRoute(
                  "GetUnit",
                  new { Id = unit.ID },
                  unit);
        }

        // PUT: api/Unit/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Unit unit)
        {
            if (unit == null)
            {
                return BadRequest("Unit is null.");
            }

            Unit unitToUpdate = _dataRepository.Get(id);
            if (unitToUpdate == null)
            {
                return NotFound("The Unit record couldn't be found.");
            }

            _dataRepository.Update(unitToUpdate, unit);
            return NoContent();
        }

        // DELETE: api/Unit/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Unit unit = _dataRepository.Get(id);
            if (unit == null)
            {
                return NotFound("The Unit record couldn't be found.");
            }

            _dataRepository.Delete(unit);
            return NoContent();
        }
    }
}