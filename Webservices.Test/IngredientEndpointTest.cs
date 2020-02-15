using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using Webservices.Controllers;
using Webservices.Models;
using Webservices.Models.DataManager;
using Webservices.Models.Repository;
using Xunit;

namespace Webservices.Test
{
    public class IngredientEndpointTest
    {
        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);

            Ingredient tempIngredient = new Ingredient()
            {
                IsValid = true,
                Name = "TestIngredient"
            };

            // Act
            IActionResult actionResult = ingredientCont.Post(tempIngredient);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestIngredient", ((Ingredient)createdResult.Value).Name);
            Assert.Equal(0, ((Ingredient)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            FilterModel fm = new FilterModel();
            // Act
            var okResult = ingredientCont.Get(fm);

            // Assert
            Assert.IsType<ActionResult<PagedCollectionResponse<Ingredient>>>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions<RepositoryContext> options = new MockDBHandler().IngredientWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Ingredient> mockRepository = new IngredientManager(context);
                IngredientController ingredientCont = new IngredientController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act

                var okResult = ingredientCont.Get(fm) as ActionResult<PagedCollectionResponse<Ingredient>>;

                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Ingredient>>>(okResult);
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().IngredientWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Ingredient> mockRepository = new IngredientManager(context);
                IngredientController ingredientCont = new IngredientController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act
                Ingredient newIngredient = new Ingredient() { IsValid = false, Name = "Changed" };
                var putResult = ingredientCont.Put(2, newIngredient) as OkObjectResult;
                var okResult = ingredientCont.Get(fm);
                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Ingredient>>>(okResult);
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
                Assert.False(retObj.Value.Items.ToList()[1].IsValid);
                Assert.Equal("Changed", retObj.Value.Items.ToList()[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            // Act
            Ingredient newIngredient = new Ingredient() { IsValid = false, Name = "Changed" };
            var notFoundResult = ingredientCont.Put(68, newIngredient);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);

        }

        [Fact]
        public void Put_Null_Should_ReturnsBadRequest()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            // Act
            Ingredient newIngredient = null;
            var badRequest = ingredientCont.Put(68, newIngredient);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(badRequest);

        }

        [Fact]
        public void GetById_UnknownintPassed_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            // Act
            var notFoundResult = ingredientCont.Get(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public void Delete_InvalidID_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            // Act
            var notFoundResult = ingredientCont.Delete(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }
        [Fact]
        public void Delete_ShouldWork()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().IngredientWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Ingredient> mockRepository = new IngredientManager(context);
                IngredientController ingredientCont = new IngredientController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act                
                var putResult = ingredientCont.Delete(2) as OkObjectResult;
                var okResult = ingredientCont.Get(fm);
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Ingredient>>>(okResult);
                // Assert                  
                Assert.Equal(2, retObj.Value.Items.ToList().Count);

                Assert.Equal("Ing1", retObj.Value.Items.ToList()[0].Name);
                Assert.Equal("Ing3", retObj.Value.Items.ToList()[1].Name);
            }

        }
    }
}
