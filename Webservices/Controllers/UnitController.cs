using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Models;
using Webservices.Models.Repository;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IDataRepository<Unit> _dataRepository;
        public UnitController(IDataRepository<Unit> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Unit
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Unit> categories = _dataRepository.GetAll();
            return Ok(categories);
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
                  "Get",
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