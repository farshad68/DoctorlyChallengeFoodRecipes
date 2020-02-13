using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webservices.Models;

namespace Webservices.Test
{
    public class MockDBHandler : IMockDBHandler
    {
        private DbContextOptions _options;
        private string _dbName;
        public MockDBHandler()
        {
            _dbName = "Database" + Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;
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
        public async System.Threading.Tasks.Task<IMockDBHandler> ReciptWithThreeMemberAsync()
        {
            using (var context = new RepositoryContext(_options))
            {
                ICollection<RecipeIngredient> recIng1 = new List<RecipeIngredient>();
                recIng1.Add(new RecipeIngredient
                {
                    Ingredient =  context.Ingredient.FirstAsync().Result,
                    Quantity = 10,
                    Unit = context.Unit.FirstAsync().Result
                });
                context.Recipe.Add(new Recipe
                {
                    CaloriesPerServing = 100,
                    Category =  context.Category.FirstAsync().Result,
                    Country =  context.Country.FirstAsync().Result,
                    Description = "Desc 1",
                    Direction = "Dir 1",
                    IsCompleted = false,
                    Name = "Nam 1",
                    NumberOfServing = 1,
                    PreparationTime = 6000,
                    Token = Guid.NewGuid(),
                    Year = 2015,
                    Ingredients = recIng1
                });
                context.SaveChanges();
            }
            return this;
        }
        public DbContextOptions build()
        {
            return _options;
        }
    }
}
