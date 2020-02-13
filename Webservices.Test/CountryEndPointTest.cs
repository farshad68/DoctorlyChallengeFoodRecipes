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
                Name = "TestCat"
            };

            // Act
            IActionResult actionResult = countryCont.Post(tempCountry);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestCat", ((Country)createdResult.Value).Name);
            Assert.Equal(0, ((Country)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            // Act
            var okResult = countryCont.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions options = new MockDBHandler().CountryWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Country> mockRepository = new CountryManager(context);
                CountryController countryCont = new CountryController(mockRepository);
                //Act
                var okResult = countryCont.Get() as OkObjectResult;

                // Assert  

                var items = Assert.IsType<List<Country>>(okResult.Value);
                Assert.Equal(3, items.Count);
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions options = new MockDBHandler().CountryWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Country> mockRepository = new CountryManager(context);
                CountryController countryCont = new CountryController(mockRepository);
                //Act
                Country newCat = new Country() { IsValid = false, Name = "Changed" };
                var putResult = countryCont.Put(2, newCat) as OkObjectResult;
                var okResult = countryCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Country>>(okResult.Value);
                Assert.Equal(3, items.Count);
                Assert.False(items[1].IsValid);
                Assert.Equal("Changed", items[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Country> mockRepository = Substitute.For<IDataRepository<Country>>();
            CountryController countryCont = new CountryController(mockRepository);
            // Act
            Country newCat = new Country() { IsValid = false, Name = "Changed" };
            var notFoundResult = countryCont.Put(68, newCat);

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
            Country newCat = null;
            var badRequest = countryCont.Put(68, newCat);

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
            DbContextOptions options = new MockDBHandler().CountryWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Country> mockRepository = new CountryManager(context);
                CountryController countryCont = new CountryController(mockRepository);
                //Act                
                var putResult = countryCont.Delete(2) as OkObjectResult;
                var okResult = countryCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Country>>(okResult.Value);
                Assert.Equal(2, items.Count);
                Assert.Equal("Country1", items[0].Name);
                Assert.Equal("Country3", items[1].Name);
            }

        }
    }
}
