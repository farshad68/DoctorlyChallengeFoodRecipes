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
    public class CountryEndPointTest
    {
        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);

            Country tempCountry = new Country()
            {
                IsValid = true,
                Name = "TestCountry"
            };

            // Act
            IActionResult actionResult = countryCont.Post(tempCountry);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestCountry", ((Country)createdResult.Value).Name);
            Assert.Equal(0, ((Country)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            FilterModel fm = new FilterModel();
            // Act

            var okResult = countryCont.Get(fm);

            // Assert
            Assert.IsType<ActionResult<PagedCollectionResponse<Country>>>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions<RepositoryContext> options = new MockDBHandler().CountryWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Country> mockRepository = new CountryManager(context);
                CountryController countryCont = new CountryController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act

                var okResult = countryCont.Get(fm) as ActionResult<PagedCollectionResponse<Country>>;

                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Country>>>(okResult);
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CountryWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Country> mockRepository = new CountryManager(context);
                CountryController countryCont = new CountryController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act
                Country newCountry = new Country() { IsValid = false, Name = "Changed" };
                var putResult = countryCont.Put(2, newCountry) as OkObjectResult;
                var okResult = countryCont.Get(fm);
                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Country>>>(okResult);
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
                Assert.False(retObj.Value.Items.ToList()[1].IsValid);
                Assert.Equal("Changed", retObj.Value.Items.ToList()[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            // Act
            Country newCountry = new Country() { IsValid = false, Name = "Changed" };
            var notFoundResult = countryCont.Put(68, newCountry);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);

        }

        [Fact]
        public void Put_Null_Should_ReturnsBadRequest()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            // Act
            Country newCountry = null;
            var badRequest = countryCont.Put(68, newCountry);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(badRequest);

        }

        [Fact]
        public void GetById_UnknownintPassed_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            // Act
            var notFoundResult = countryCont.Get(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public void Delete_InvalidID_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            // Act
            var notFoundResult = countryCont.Delete(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }
        [Fact]
        public void Delete_ShouldWork()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CountryWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Country> mockRepository = new CountryManager(context);
                CountryController countryCont = new CountryController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act                
                var putResult = countryCont.Delete(2) as OkObjectResult;
                var okResult = countryCont.Get(fm);
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Country>>>(okResult);
                // Assert                  
                Assert.Equal(2, retObj.Value.Items.ToList().Count);
                Assert.Equal("Country1", retObj.Value.Items.ToList()[0].Name);
                Assert.Equal("Country3", retObj.Value.Items.ToList()[1].Name);
            }

        }
    }
}
