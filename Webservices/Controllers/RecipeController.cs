using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Models;
using Webservices.Models.Repository;
using Webservices.ViewModel;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IDataRepository<Recipe> _dataRepository;
        public RecipeController(IDataRepository<Recipe> dataRepository, IMapper mapper)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
        }
        // GET: api/Recipe
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Recipe> recipe = _dataRepository.GetAll();
            
            IEnumerable<RecipeViewModel> icollectionDest = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeViewModel>>(recipe);
            return Ok(icollectionDest);
        }
        // GET: api/Recipe/5
        [HttpGet("{id}", Name = "GetRecipe")]
        public IActionResult Get(long id)
        {
            Recipe recipe = _dataRepository.Get(id);

            if (recipe == null)
            {
                return NotFound("The recipe record couldn't be found.");
            }

            return Ok(recipe);
        }

        // POST: api/recipe
        [HttpPost]
        public IActionResult Post([FromBody] Recipe recipe)
        {
            if (recipe == null)
            {
                return BadRequest("recipe is null.");
            }

            _dataRepository.Add(recipe);
            return CreatedAtRoute(
                  "Get",
                  new { Id = recipe.ID },
                  recipe);
        }

        // POST: api/recipe
        [HttpPost]
        [Route("addRecipeSimple")]
        public IActionResult PostAddSimpleRecipe([FromBody] RecipeViewModel recipe)
        {
            if (recipe == null)
            {
                return BadRequest("recipe is null.");
            }
            Recipe recipeDest = _mapper.Map<RecipeViewModel, Recipe  >(recipe);
            _dataRepository.Add(recipeDest);
            return CreatedAtRoute(
                  "Get",
                  new { Id = recipe.ID },
                  recipe);
        }
        // PUT: api/Recipe/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Recipe recipe)
        {
            if (recipe == null)
            {
                return BadRequest("Recipe is null.");
            }

            Recipe recipeToUpdate = _dataRepository.Get(id);
            if (recipeToUpdate == null)
            {
                return NotFound("The Recipe record couldn't be found.");
            }

            _dataRepository.Update(recipeToUpdate, recipe);
            return NoContent();
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Recipe recipe = _dataRepository.Get(id);
            if (recipe == null)
            {
                return NotFound("The Recipe record couldn't be found.");
            }

            _dataRepository.Delete(recipe);
            return NoContent();
        }

    }
}