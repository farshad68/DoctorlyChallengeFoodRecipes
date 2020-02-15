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
                Name = "TestUnit"
            };

            // Act
            IActionResult actionResult = unitCont.Post(tempUnit);
            var createdResult = actionResult as CreatedAtRouteResult;

            // Assert  
            Assert.NotNull(createdResult);
            Assert.Equal("TestUnit", ((Unit)createdResult.Value).Name);
            Assert.Equal(0, ((Unit)createdResult.Value).ID);

        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            FilterModel fm = new FilterModel();
            // Act
            var okResult = unitCont.Get(fm);

            // Assert
            Assert.IsType<ActionResult<PagedCollectionResponse<Unit>>>(okResult);
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange

            DbContextOptions<RepositoryContext> options = new MockDBHandler().UnitWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Unit> mockRepository = new UnitManager(context);
                UnitController unitCont = new UnitController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act

                var okResult = unitCont.Get(fm) as ActionResult<PagedCollectionResponse<Unit>>;

                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Unit>>>(okResult);
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
            }

        }
        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().UnitWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Unit> mockRepository = new UnitManager(context);
                UnitController unitCont = new UnitController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act
                Unit newUnit = new Unit() { IsValid = false, Name = "Changed" };
                var putResult = unitCont.Put(2, newUnit) as OkObjectResult;
                var okResult = unitCont.Get(fm);
                // Assert  
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Unit>>>(okResult);
                Assert.Equal(3, retObj.Value.Items.ToList().Count);
                Assert.False(retObj.Value.Items.ToList()[1].IsValid);
                Assert.Equal("Changed", retObj.Value.Items.ToList()[1].Name);

            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            IDataRepository<Unit> mockRepository = Substitute.For<IDataRepository<Unit>>();
            UnitController unitCont = new UnitController(mockRepository);
            // Act
            Unit newUnit = new Unit() { IsValid = false, Name = "Changed" };
            var notFoundResult = unitCont.Put(68, newUnit);

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
            Unit newUnit = null;
            var badRequest = unitCont.Put(68, newUnit);

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
            DbContextOptions<RepositoryContext> options = new MockDBHandler().UnitWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Unit> mockRepository = new UnitManager(context);
                UnitController unitCont = new UnitController(mockRepository);
                FilterModel fm = new FilterModel();
                //Act                
                var putResult = unitCont.Delete(2) as OkObjectResult;
                var okResult = unitCont.Get(fm);
                // Assert 
                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Unit>>>(okResult);                                 
                Assert.Equal(2, retObj.Value.Items.ToList().Count);
                Assert.Equal("Unit1", retObj.Value.Items.ToList()[0].Name);
                Assert.Equal("Unit3", retObj.Value.Items.ToList()[1].Name);
            }

        }
    }
}
