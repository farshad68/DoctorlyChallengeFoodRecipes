﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Webservices.Controllers;
using Webservices.Models;
using Webservices.Models.DataManager;
using Webservices.Models.Repository;
using Xunit;

namespace Webservices.Test
{
    public class RecipeEndPointTest
    {
        private readonly IMapper _mapper;
        public RecipeEndPointTest(IMapper mapper)
        {
            _mapper = mapper;
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange


            DbContextOptions options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMemberAsync().Result.build();            
            using (var context = new RepositoryContext(options))
            {
                IDataRepository<Recipe> mockRepository = new RecipeManager(context);
                 
                RecipeController recipecontroller = new RecipeController(mockRepository,_mapper);
                //Act
                var okResult = recipecontroller.Get() as OkObjectResult;

                // Assert  

                var items = Assert.IsType<List<Recipe>>(okResult.Value);
                Assert.Single(items);
            }

        }
    }
}