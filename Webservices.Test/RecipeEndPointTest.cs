using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webservices.Controllers;
using Webservices.Mapper;
using Webservices.Models;
using Webservices.Models.DataManager;
using Webservices.Models.Repository;
using Webservices.ViewModel;
using Xunit;

namespace Webservices.Test
{
    public class RecipeEndPointTest
    {
        private  IMapper _mapper;
        //private  ICustomMapper _customMapper;



        public RecipeEndPointTest()
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();            
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();            
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);
                 
                RecipeController recipecontroller = new RecipeController(mockRepository,_mapper,_customMapper);
                
                // Act
                IActionResult actionResult = recipecontroller.Post(new MockDBHandler().buildMockRecipeView());
                var createdResult = actionResult as CreatedAtRouteResult;
                // Assert  
                Assert.NotNull(createdResult);
                Assert.Equal("N", ((RecipeViewModel)createdResult.Value).Name);
                Assert.Equal(2, ((RecipeViewModel)createdResult.Value).Ingredients.Count);

            }

        }
       

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                // Arrange
                IDataRepository<Recipe> mockRepository = Substitute.For<IDataRepository<Recipe>>();
                RecipeController recipeCont = new RecipeController(mockRepository, _mapper, _customMapper);
                FilterModel fm = new FilterModel();
                // Act

                var okResult = recipeCont.Get(fm);

                // Assert
                Assert.IsType<ActionResult<PagedCollectionResponse<Recipe>>>(okResult);
            }
        }

        [Fact]
        public void Add_ValidObject_Then_Get_Should_bring_Something()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                FilterModel fm = new FilterModel();
                //Act
                recipecontroller.Post(new MockDBHandler().buildMockRecipeView());
                var okResult = recipecontroller.Get(fm);

                // Assert  

                var retObj = Assert.IsType<ActionResult<PagedCollectionResponse<Recipe>>>(okResult);
                Assert.Equal(2, retObj.Value.Items.ToList().Count);
            }

        }

        [Fact]
        public void After_Edit_ValidObject_Get_Should_Changed()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                //Act
                var forEdit = new MockDBHandler().buildMockRecipeView();
                forEdit.ID = 1; // Because In mock it is something else and in equalation assert result to false
                OkObjectResult okresult = recipecontroller.Get(1) as OkObjectResult;
                RecipeViewModel origin = okresult.Value as RecipeViewModel;
                recipecontroller.Put(origin.ID,forEdit);

                OkObjectResult okresultAfterEdit = recipecontroller.Get(1) as OkObjectResult;
                RecipeViewModel afterEdit = okresultAfterEdit.Value as RecipeViewModel;

                // Assert  
                
                Assert.True(forEdit.Equals(afterEdit));
                Assert.False(origin.Equals(afterEdit));
            }

        }

        [Fact]
        public void Put_Invalid_ID_Should_ReturnsNotFoundResult()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                //Act
                var forEdit = new MockDBHandler().buildMockRecipeView();
                forEdit.ID = 1; // Because In mock it is something else and in equalation assert result to false

                var notFoundResult = recipecontroller.Put(68, forEdit);

                // Assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
                
            }
                    }

        [Fact]
        public void Put_Null_Should_ReturnsBadRequest()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                //Act


                var badRequest = recipecontroller.Put(1, null);

                // Assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(badRequest);

            }            

        }

        [Fact]
        public void GetById_UnknownintPassed_ReturnsNotFoundResult()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                //Act                
                var notFoundResult = recipecontroller.Get(68) ;

                // Assert  
                Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
            }            
        }

        [Fact]
        public void Delete_InvalidID_ReturnsNotFoundResult()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                //Act                
                var notFoundResult = recipecontroller.Delete(68);

                // Assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(notFoundResult);
            }
            
            
        }
        [Fact]
        public void Delete_ShouldWork()
        {
            // Arrange
            DbContextOptions<RepositoryContext> options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();
            using (var context = new RepositoryContext(options))
            {
                ICustomMapper _customMapper = new CustomMapper(context);
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);

                RecipeController recipecontroller = new RecipeController(mockRepository, _mapper, _customMapper);
                FilterModel fm = new FilterModel();
                //Act                
                var okResultBeforDelete = recipecontroller.Get(fm);               
                var itemsBeforeDelete  = Assert.IsType<ActionResult<PagedCollectionResponse<Recipe>>>(okResultBeforDelete);
                Assert.Equal(1, itemsBeforeDelete.Value.Items.ToList().Count);
                var notFoundResult = recipecontroller.Delete(1);
                var okResultAfterDelete = recipecontroller.Get(fm);
                var itemsAfterDelete = Assert.IsType<ActionResult<PagedCollectionResponse<Recipe>>>(okResultAfterDelete);
                Assert.Equal(0, itemsAfterDelete.Value.Items.ToList().Count);                
            
                                        
            }

        }

    }
}
