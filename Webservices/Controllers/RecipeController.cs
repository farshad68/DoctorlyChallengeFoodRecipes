﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using Webservices.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RecipeController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IDataRepository<Recipe> _dataRepository;
        private readonly ICustomMapper _custumMapper;
        UserManager<ApplicationUser> _userManager;
        public RecipeController(IDataRepository<Recipe> dataRepository, IMapper mapper, ICustomMapper custumMapper, UserManager<ApplicationUser> userManager)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _custumMapper = custumMapper;
            _userManager = userManager;
        }
        // GET: api/Recipe
        [HttpGet]
        public ActionResult<PagedCollectionResponse<RecipeViewModel>> Get([FromQuery] FilterModel filter)        
        {
            IEnumerable<Recipe> recipe = _dataRepository.GetAll();
            List<RecipeViewModel> recipDes = new List<RecipeViewModel>();
            
            //Filtering logic  
            Func<FilterModel, IEnumerable<Recipe>> filterData = (filterModel) =>
            {
                return recipe.Where(p => p.Name.StartsWith(filterModel.Term ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)                )
                .Skip((filterModel.Page - 1) * filter.Limit)
                .Take(filterModel.Limit);
            };

            foreach (var item in filterData(filter))
            {
                recipDes.Add(_custumMapper.Map(item));
            }

            //Get the data for the current page  
            var result = new PagedCollectionResponse<RecipeViewModel>();
            result.Items = recipDes; 

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
            //recipe.Token = Guid.NewGuid();
            Recipe recipeDest = _custumMapper.Map(recipe);
            //Recipe recipeDest = _mapper.Map<RecipeViewModel, Recipe  >(recipe);
            var name = _userManager.GetUserId(User);
            var user =  _userManager.FindByNameAsync(name).Result;
            
            recipeDest.User = user;
            recipeDest.UserId = user.Id;

            if (_dataRepository.Exist(recipeDest))
            {
               return BadRequest("This recipe exist.");
            }

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

            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            if (!roles.Contains("Editor"))
            {
                var name = _userManager.GetUserId(User);
                var user = _userManager.FindByNameAsync(name).Result;

                if (recipeToUpdate.User != user)
                {
                    return BadRequest("Recipe is not belong to this user.");
                }
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