using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Mapper;
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
        private readonly ICustomMapper _custumMapper;
        public RecipeController(IDataRepository<Recipe> dataRepository, IMapper mapper, ICustomMapper custumMapper)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _custumMapper = custumMapper;
        }
        // GET: api/Recipe
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Recipe> recipe = _dataRepository.GetAll();
            List<RecipeViewModel> recipDes = new List<RecipeViewModel>();

            foreach(var item in recipe)
            {
                recipDes.Add(_custumMapper.Map(item));
            }
                        
            return Ok(recipDes);
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
            var recipDes = _custumMapper.Map(recipe);
            return Ok(recipDes);
        }

        // POST: api/recipe
        //[HttpPost]
        //public IActionResult Post([FromBody] Recipe recipe)
        //{
        //    if (recipe == null)
        //    {
        //        return BadRequest("recipe is null.");
        //    }

        //    _dataRepository.Add(recipe);
        //    return CreatedAtRoute(
        //          "Get",
        //          new { Id = recipe.ID },
        //          recipe);
        //}

        // POST: api/recipe
        [HttpPost]
        //[Route("addRecipeSimple")]
        public IActionResult Post([FromBody] RecipeViewModel recipe)
        {
            if (recipe == null)
            {
                return BadRequest("recipe is null.");
            }
            recipe.Token = Guid.NewGuid();
            Recipe recipeDest = _custumMapper.Map(recipe);
            //Recipe recipeDest = _mapper.Map<RecipeViewModel, Recipe  >(recipe);
            
            
            _dataRepository.Add(recipeDest);
            recipe.ID = recipeDest.ID;
            return CreatedAtRoute(
                  "GetRecipe",
                  new { Id = recipe.ID },
                  recipe);
        }
        // PUT: api/Recipe/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] RecipeViewModel recipeVM)
        {
            if (recipeVM == null)
            {
                return BadRequest("Recipe is null.");
            }

            Recipe recipeToUpdate = _dataRepository.Get(id);
            if (recipeToUpdate == null)
            {
                return NotFound("The Recipe record couldn't be found.");
            }
            Recipe recipe = _custumMapper.Map(recipeVM);
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