using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
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
                Name = "TestCat"
            };

            // Act
            IActionResult actionResult = ingredientCont.Post(tempIngredient);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestCat", ((Ingredient)createdResult.Value).Name);
            Assert.Equal(0, ((Ingredient)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            // Act
            var okResult = ingredientCont.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions options = new MockDBHandler().IngredientWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Ingredient> mockRepository = new IngredientManager(context);
                IngredientController ingredientCont = new IngredientController(mockRepository);
                //Act
                var okResult = ingredientCont.Get() as OkObjectResult;

                // Assert  

                var items = Assert.IsType<List<Ingredient>>(okResult.Value);
                Assert.Equal(3, items.Count);
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions options = new MockDBHandler().IngredientWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Ingredient> mockRepository = new IngredientManager(context);
                IngredientController ingredientCont = new IngredientController(mockRepository);
                //Act
                Ingredient newCat = new Ingredient() { IsValid = false, Name = "Changed" };
                var putResult = ingredientCont.Put(2, newCat) as OkObjectResult;
                var okResult = ingredientCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Ingredient>>(okResult.Value);
                Assert.Equal(3, items.Count);
                Assert.False(items[1].IsValid);
                Assert.Equal("Changed", items[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Ingredient> mockRepository = Substitute.For<IDataRepository<Ingredient>>();
            IngredientController ingredientCont = new IngredientController(mockRepository);
            // Act
            Ingredient newCat = new Ingredient() { IsValid = false, Name = "Changed" };
            var notFoundResult = ingredientCont.Put(68, newCat);

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
            Ingredient newCat = null;
            var badRequest = ingredientCont.Put(68, newCat);

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
            DbContextOptions options = new MockDBHandler().IngredientWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Ingredient> mockRepository = new IngredientManager(context);
                IngredientController ingredientCont = new IngredientController(mockRepository);
                //Act                
                var putResult = ingredientCont.Delete(2) as OkObjectResult;
                var okResult = ingredientCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Ingredient>>(okResult.Value);
                Assert.Equal(2, items.Count);
                Assert.Equal("Ing1", items[0].Name);
                Assert.Equal("Ing3", items[1].Name);
            }

        }
    }
}
