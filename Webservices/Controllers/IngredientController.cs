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
    public class IngredientController : ControllerBase
    {
        private readonly IDataRepository<Ingredient> _dataRepository;
        public IngredientController(IDataRepository<Ingredient> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Ingredient
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Ingredient> categories = _dataRepository.GetAll();
            return Ok(categories);
        }
        // GET: api/Ingredient/5
        [HttpGet("{id}", Name = "GetIngredient")]
        public IActionResult Get(long id)
        {
            Ingredient ingredient = _dataRepository.Get(id);

            if (ingredient == null)
            {
                return NotFound("The ingredient record couldn't be found.");
            }

            return Ok(ingredient);
        }

        // POST: api/Ingredient
        [HttpPost]
        public IActionResult Post([FromBody] Ingredient ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest("ingredient is null.");
            }

            _dataRepository.Add(ingredient);
            return CreatedAtRoute(
                  "GetIngredient",
                  new { Id = ingredient.ID },
                  ingredient);
        }

        // PUT: api/Ingredient/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Ingredient ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest("Ingredient is null.");
            }

            Ingredient ingredientToUpdate = _dataRepository.Get(id);
            if (ingredientToUpdate == null)
            {
                return NotFound("The Ingredient record couldn't be found.");
            }

            _dataRepository.Update(ingredientToUpdate, ingredient);
            return NoContent();
        }

        // DELETE: api/Ingredient/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Ingredient ingredient = _dataRepository.Get(id);
            if (ingredient == null)
            {
                return NotFound("The Ingredient record couldn't be found.");
            }

            _dataRepository.Delete(ingredient);
            return NoContent();
        }
    }
}