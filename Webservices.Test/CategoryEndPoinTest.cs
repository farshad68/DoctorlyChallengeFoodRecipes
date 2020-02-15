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
    public class CategoryEndPoinTest
    {
         
        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            IDataRepository<Category> mockRepository = Substitute.For<IDataRepository<Category>>();
            CategoryController categoryCont = new CategoryController(mockRepository);

            Category tempCategory = new Category()
            {
                IsValid = true,
                Name = "TestCat"
            };

            // Act
            IActionResult actionResult = categoryCont.Post(tempCategory);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestCat", ((Category)createdResult.Value).Name);
            Assert.Equal(0, ((Category)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Category> mockRepository = Substitute.For<IDataRepository<Category>>();
            CategoryController categoryCont = new CategoryController(mockRepository);
            // Act
            var okResult = categoryCont.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions<RepositoryContext> options =new MockDBHandler().CategoryWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Category> mockRepository = new CategoryManager(context);
                CategoryController categoryCont = new CategoryController(mockRepository);
                //Act
                var okResult = categoryCont.Get() as OkObjectResult;
                
                // Assert  

                var items = Assert.IsType<List<Category>>(okResult.Value);
                Assert.Equal(3, items.Count);                
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Category> mockRepository = new CategoryManager(context);
                CategoryController categoryCont = new CategoryController(mockRepository);
                //Act
                Category newCat = new Category() { IsValid = false, Name = "Changed" };
                var putResult = categoryCont.Put(2,newCat) as OkObjectResult;
                var okResult = categoryCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Category>>(okResult.Value);
                Assert.Equal(3, items.Count);
                Assert.False(items[1].IsValid);
                Assert.Equal("Changed", items[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Category> mockRepository = Substitute.For<IDataRepository<Category>>();
            CategoryController categoryCont = new CategoryController(mockRepository);
            // Act
            Category newCat = new Category() { IsValid = false, Name = "Changed" };
            var notFoundResult = categoryCont.Put(68,newCat);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);            

        }

        [Fact]
        public void Put_Null_Should_ReturnsBadRequest()
        {
            // Arrange
            IDataRepository<Category> mockRepository = Substitute.For<IDataRepository<Category>>();
            CategoryController categoryCont = new CategoryController(mockRepository);
            // Act
            Category newCat = null;
            var badRequest = categoryCont.Put(68, newCat);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(badRequest);

        }

        [Fact]
        public void GetById_UnknownintPassed_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Category> mockRepository = Substitute.For<IDataRepository<Category>>();
            CategoryController categoryCont = new CategoryController(mockRepository);
            // Act
            var notFoundResult = categoryCont.Get(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public void Delete_InvalidID_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Category> mockRepository = Substitute.For<IDataRepository<Category>>();
            CategoryController categoryCont = new CategoryController(mockRepository);
            // Act
            var notFoundResult = categoryCont.Delete(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }
        [Fact]
        public void Delete_ShouldWork()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Category> mockRepository = new CategoryManager(context);
                CategoryController categoryCont = new CategoryController(mockRepository);
                //Act                
                var putResult = categoryCont.Delete(2) as OkObjectResult;
                var okResult = categoryCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Category>>(okResult.Value);
                Assert.Equal(2, items.Count);                
                Assert.Equal("cat1", items[0].Name);
                Assert.Equal("cat3", items[1].Name);
            }

        }
    }
}
