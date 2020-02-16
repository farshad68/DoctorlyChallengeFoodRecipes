using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Models;
using Webservices.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        public ActionResult<PagedCollectionResponse<Category>> Get([FromQuery] FilterModel filter)
        {
            IEnumerable<Category> categories = _dataRepository.GetAll();
            //Filtering logic  
            Func<FilterModel, IEnumerable<Category>> filterData = (filterModel) =>
            {
                return categories.Where(p => p.Name.StartsWith(filterModel.Term ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))
                .Skip((filterModel.Page - 1) * filter.Limit)
                .Take(filterModel.Limit);
            };

            //Get the data for the current page  
            var result = new PagedCollectionResponse<Category>();
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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
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