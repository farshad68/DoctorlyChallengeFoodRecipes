using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webservices.Data;
using Webservices.Mapper;
using Webservices.Models;
using Webservices.Models.Repository;
using Webservices.ViewModel;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RecipeHelperController : ControllerBase
    {
        private readonly IDataRepository<Recipe> _dataRepository;
        private readonly IDataRepositoryTokenis<RecipeTokenLookUP> _tokenRepository;
        private readonly ICustomMapper _custumMapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<Country> _countryRepository;
        public RecipeHelperController(IDataRepository<Recipe> dataRepository, 
            IDataRepositoryTokenis<RecipeTokenLookUP> tokenRepository, 
            ICustomMapper custumMapper, 
            UserManager<ApplicationUser> userManager,
            IDataRepository<Country> countryRepository)
        {
            _dataRepository = dataRepository;
            _tokenRepository = tokenRepository;
            _custumMapper = custumMapper;
            _userManager = userManager;
            _countryRepository = countryRepository;
        }

        [HttpPost]        
        public IActionResult Post([FromBody] RecipeViewModel recipe)
        {
            if (recipe == null)
            {
                return BadRequest("recipe is null.");
            }
            //recipe.Token = Guid.NewGuid();
            FillNullvalueswithDefaultValue(ref recipe);
            Recipe recipeDest = _custumMapper.Map(recipe);
            //Recipe recipeDest = _mapper.Map<RecipeViewModel, Recipe  >(recipe);
            var name = _userManager.GetUserId(User);
            var user = _userManager.FindByNameAsync(name).Result;

            recipeDest.User = user;
            recipeDest.UserId = user.Id;

            if (_dataRepository.Exist(recipeDest))
            {
                return BadRequest("This recipe exist.");
            }

            _dataRepository.Add(recipeDest);
            recipe.ID = recipeDest.ID;

            var rtlookup = new RecipeTokenLookUP() { RecipeID = recipe.ID, Recipe = recipeDest, Token = Guid.NewGuid() };

            _tokenRepository.Add(rtlookup);
            return Ok(rtlookup.Token.ToString());
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var lookup = _tokenRepository.Get(id);

            if (lookup == null)
            {
                return NotFound("Token is not valid.");
            }

            Recipe recipe = _dataRepository.Get(lookup.RecipeID);

            if (recipe == null)
            {
                return NotFound("The recipe record couldn't be found.");
            }
            var recipDes = _custumMapper.Map(recipe);
            return Ok(recipDes);            
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] RecipeViewModel recipeVM)
        {
            if (recipeVM == null)
            {
                return BadRequest("Recipe is null.");
            }

            var lookup = _tokenRepository.Get(id);

            if (lookup == null)
            {
                return NotFound("Token is not valid.");
            }

            Recipe recipeToUpdate = _dataRepository.Get(lookup.RecipeID);            

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
            FillNullvalueswithDefaultValue(ref recipeVM);
            Recipe recipe = _custumMapper.Map(recipeVM);            

            _dataRepository.Update(recipeToUpdate, recipe);
            return NoContent();            
        }

        private void FillNotProvidedDetailsWithExistData(Recipe recipeToUpdate, ref Recipe recipe)
        {
            if (recipe.Category.Name == "Default") recipe.Category = recipeToUpdate.Category;
            if (recipe.Country.Name == "Default") recipe.Country = recipeToUpdate.Country;
            if (recipe.Description == "Default") recipe.Description = recipeToUpdate.Description;
            if (recipe.Direction == "Default") recipe.Direction = recipeToUpdate.Direction;
            if (recipe.Ingredients.Count == 0) recipe.Ingredients = recipeToUpdate.Ingredients;
            if (recipe.Name == "Default") recipe.Name = recipeToUpdate.Name;
             recipe.User = recipeToUpdate.User;
            recipe.UserId = recipeToUpdate.UserId;
        }

        private void FillNullvalueswithDefaultValue(ref RecipeViewModel recipe)
        {

            if (recipe.CountryName == null) {
                Country country = new Country() { IsValid = true, Name = "Default" };
                List<Country> ca = _countryRepository.GetAll().Where(T => T.Name == "Default").ToList() ;
                if (ca.Count == 0)
                {
                    _countryRepository.Add(country);
                    recipe.CountryName = country.Name;
                }
                else
                {
                    recipe.CountryName = ca.First().Name;
                }
            }

            if (recipe.CategoryName == null) recipe.Name = "Default";
            if (recipe.Description == null) recipe.Description = "Default";
            if (recipe.Direction == null) recipe.Direction = "Default";
            if (recipe.Ingredients == null) recipe.Ingredients = new List<IngredientViewModel>();
            if (recipe.Name == null) recipe.Name = "Default";           
        }
    }
}