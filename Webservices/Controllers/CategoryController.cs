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
    public class CategoryController : ControllerBase
    {
        private readonly IDataRepository<Category> _dataRepository;
        public CategoryController(IDataRepository<Category> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Category
        /// <summary>
        /// Get API Value
        /// </summary>
        /// <remarks>This API will get the Categories.</remarks>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Category> categories = _dataRepository.GetAll();
            return Ok(categories);
        }
        // GET: api/Category/5
        [HttpGet("{id}", Name = "GetCategory")]
        public IActionResult Get(long id)
        {
            Category category = _dataRepository.Get(id);

            if (category == null)
            {
                return NotFound("The category record couldn't be found.");
            }

            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("category is null.");
            }

            _dataRepository.Add(category);
            return CreatedAtRoute(
                  "GetCategory",
                  new { Id = category.ID },
                  category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category is null.");
            }

            Category categoryToUpdate = _dataRepository.Get(id);
            if (categoryToUpdate == null)
            {
                return NotFound("The Category record couldn't be found.");
            }

            _dataRepository.Update(categoryToUpdate, category);
            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Category category = _dataRepository.Get(id);
            if (category == null)
            {
                return NotFound("The Category record couldn't be found.");
            }

            _dataRepository.Delete(category);
            return NoContent();
        }

    }
}