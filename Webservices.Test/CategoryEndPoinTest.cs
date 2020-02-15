using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            FilterModel fm = new FilterModel();
            // Act
            var okResult = categoryCont.Get(fm);

            // Assert
            Assert.IsType<ActionResult<PagedCollectionResponse<Category>>>(okResult);
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
                FilterModel fm = new FilterModel();
                //Act
                var okResult = categoryCont.Get(fm) as ActionResult<PagedCollectionResponse<Category>>;

                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Category>>>(okResult);                
                Assert.Equal(3, retObj.Value.Items.ToList().Count);                
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
                FilterModel fm = new FilterModel();
                var okResult = categoryCont.Get(fm) ;
                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Category>>>(okResult);               
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
                Assert.False(retObj.Value.Items.ToList()[1].IsValid);
                Assert.Equal("Changed", retObj.Value.Items.ToList()[1].Name);

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
                FilterModel fm = new FilterModel();
                //Act                
                var putResult = categoryCont.Delete(2) as OkObjectResult;                
                var okResult = categoryCont.Get(fm);
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Category>>>(okResult);
                // Assert                  
                Assert.Equal(2, retObj.Value.Items.ToList().Count);                
                Assert.Equal("cat1", retObj.Value.Items.ToList()[0].Name);
                Assert.Equal("cat3", retObj.Value.Items.ToList()[1].Name);
            }

        }
    }
}
