using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Models;
using Webservices.Models.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class IngredientController : ControllerBase
    {
        private readonly IDataRepository<Ingredient> _dataRepository;
        public IngredientController(IDataRepository<Ingredient> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Ingredient
        [HttpGet]
        public ActionResult<PagedCollectionResponse<Ingredient>> Get([FromQuery] FilterModel filter)
        {
            IEnumerable<Ingredient> ingredient = _dataRepository.GetAll();
            //Filtering logic  
            Func<FilterModel, IEnumerable<Ingredient>> filterData = (filterModel) =>
            {
                return ingredient.Where(p => p.Name.StartsWith(filterModel.Term ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filter.Limit)
                .Take(filterModel.Limit);
            };

            //Get the data for the current page  
            var result = new PagedCollectionResponse<Ingredient>();
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
            try { 
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
            catch (Exception ex)
            {
                if (ex.InnerException.Message.StartsWith("Cannot insert duplicate key row in object"))
                    return BadRequest("Can not Insert two equal Ingredint");
                else
                    return BadRequest(ex.Message);
            }
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