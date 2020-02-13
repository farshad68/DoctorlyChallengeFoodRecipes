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
    public class UnitEndPointTest
    {
        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);

            Unit tempUnit = new Unit()
            {
                IsValid = true,
                Name = "TestCat"
            };

            // Act
            IActionResult actionResult = unitCont.Post(tempUnit);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestCat", ((Unit)createdResult.Value).Name);
            Assert.Equal(0, ((Unit)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            // Act
            var okResult = unitCont.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions options = new MockDBHandler().UnitWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Unit> mockRepository = new UnitManager(context);
                UnitController unitCont = new UnitController(mockRepository);
                //Act
                var okResult = unitCont.Get() as OkObjectResult;

                // Assert  

                var items = Assert.IsType<List<Unit>>(okResult.Value);
                Assert.Equal(3, items.Count);
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions options = new MockDBHandler().UnitWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Unit> mockRepository = new UnitManager(context);
                UnitController unitCont = new UnitController(mockRepository);
                //Act
                Unit newCat = new Unit() { IsValid = false, Name = "Changed" };
                var putResult = unitCont.Put(2, newCat) as OkObjectResult;
                var okResult = unitCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Unit>>(okResult.Value);
                Assert.Equal(3, items.Count);
                Assert.False(items[1].IsValid);
                Assert.Equal("Changed", items[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            // Act
            Unit newCat = new Unit() { IsValid = false, Name = "Changed" };
            var notFoundResult = unitCont.Put(68, newCat);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);

        }

        [Fact]
        public void Put_Null_Should_ReturnsBadRequest()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            // Act
            Unit newCat = null;
            var badRequest = unitCont.Put(68, newCat);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(badRequest);

        }

        [Fact]
        public void GetById_UnknownintPassed_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            // Act
            var notFoundResult = unitCont.Get(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public void Delete_InvalidID_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            // Act
            var notFoundResult = unitCont.Delete(68);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
        }
        [Fact]
        public void Delete_ShouldWork()
        {
            // Arrange
            DbContextOptions options = new MockDBHandler().UnitWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Unit> mockRepository = new UnitManager(context);
                UnitController unitCont = new UnitController(mockRepository);
                //Act                
                var putResult = unitCont.Delete(2) as OkObjectResult;
                var okResult = unitCont.Get() as OkObjectResult;
                // Assert  

                var items = Assert.IsType<List<Unit>>(okResult.Value);
                Assert.Equal(2, items.Count);
                Assert.Equal("Unit1", items[0].Name);
                Assert.Equal("Unit3", items[1].Name);
            }

        }
    }
}
