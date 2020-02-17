using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webservices.Models;
using Webservices.ViewModel;

namespace Webservices.Test
{
    public class MockDBHandler : IMockDBHandler
    {
        private DbContextOptions<RepositoryContext> _options;
        private string _dbName;
        public MockDBHandler()
        {
            _dbName = "Database" + Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;
        }
        public IMockDBHandler EmptyDB()
        {
            _options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;
            return this;
        }
        public IMockDBHandler CategoryWithThreeMember()
        {
            //.UseInMemoryDatabase(databaseName: "CategoryLisDatabase" + Guid.NewGuid().ToString())


            using (var context = new RepositoryContext(_options))
            {
                context.Category.Add(new Category { IsValid = true, Name = "cat1" });
                context.Category.Add(new Category { IsValid = true, Name = "cat2" });
                context.Category.Add(new Category { IsValid = true, Name = "cat3" });
                context.SaveChanges();
            }
            return this;
        }
        public IMockDBHandler CountryWithThreeMember()
        {
            using (var context = new RepositoryContext(_options))
            {
                context.Country.Add(new Country { IsValid = true, Name = "Country1" });
                context.Country.Add(new Country { IsValid = true, Name = "Country2" });
                context.Country.Add(new Country { IsValid = true, Name = "Country3" });
                context.SaveChanges();
            }
            return this;
        }
        public IMockDBHandler UnitWithThreeMember()
        {
            using (var context = new RepositoryContext(_options))
            {
                context.Unit.Add(new Unit { IsValid = true, Name = "Unit1" });
                context.Unit.Add(new Unit { IsValid = true, Name = "Unit2" });
                context.Unit.Add(new Unit { IsValid = true, Name = "Unit3" });
                context.SaveChanges();
            }
            return this;
        }
        public IMockDBHandler IngredientWithThreeMember()
        {
            using (var context = new RepositoryContext(_options))
            {
                context.Ingredient.Add(new Ingredient { IsValid = true, Name = "Ing1" });
                context.Ingredient.Add(new Ingredient { IsValid = true, Name = "Ing2" });
                context.Ingredient.Add(new Ingredient { IsValid = true, Name = "Ing3" });
                context.SaveChanges();
            }
            return this;
        }
        public IMockDBHandler ReciptWithThreeMember()
        {
            using (var context = new RepositoryContext(_options))
            {
                ICollection<RecipeIngredient> recIng1 = new List<RecipeIngredient>();
                var in1 = new RecipeIngredient
                {
                    Ingredient = context.Ingredient.FirstAsync().Result,
                    Quantity = 10,
                    Unit = context.Unit.FirstAsync().Result
                };
                

                var rec1 = new Recipe
                {
                    CaloriesPerServing = 100,
                    Category = context.Category.FirstAsync().Result,
                    Country = context.Country.FirstAsync().Result,
                    Description = "Desc 1",
                    Direction = "Dir 1",
                    IsCompleted = false,
                    Name = "Nam 1",
                    NumberOfServing = 1,
                    PreparationTime = 6000,                    
                    Year = 2015,
                    Ingredients = recIng1
                };
                context.Recipe.Add(rec1);
                in1.Recipe = rec1;
                recIng1.Add(in1);
                
                context.SaveChanges();
            }
            return this;
        }
        public DbContextOptions<RepositoryContext> build()
        {
            return _options;
        }

        public RecipeViewModel buildMockRecipeView()
        {
            RecipeViewModel acctual = new RecipeViewModel();
            acctual.CaloriesPerServing = 100;
            acctual.CategoryID = 3;
            acctual.CategoryName = "cat3";
            acctual.CountryID = 1;
            acctual.CountryName = "Country1";
            acctual.Description = "awdawd0";
            acctual.Direction = "13123klnkle112";
            acctual.ID = 12;
            acctual.Ingredients = new List<IngredientViewModel>();
            acctual.IsCompleted = true;
            acctual.Name = "N";
            acctual.NumberOfServing = 3;
            acctual.PreparationTime = 8000;            
            acctual.Year = 2019;

            IngredientViewModel ivm1 = new IngredientViewModel()
            {
                IngredientID = 1,
                IngredientName = "Ing1",
                Quantity = (float)0.5,
                UnitID = 1,
                UnitName = "Unit1"
            };
            acctual.Ingredients.Add(ivm1);
            IngredientViewModel ivm2 = new IngredientViewModel()
            {
                IngredientID = 2,
                IngredientName = "Ing2",
                Quantity = (float)1,
                UnitID = 2,
                UnitName = "Unit2"
            };
            acctual.Ingredients.Add(ivm2);
            return acctual;
        }

    }
}
