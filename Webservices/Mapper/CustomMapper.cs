using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models;
using Webservices.ViewModel;

namespace Webservices.Mapper
{
    public class CustomMapper
    {
        readonly RepositoryContext _repositoryContext;
        public CustomMapper(RepositoryContext context)
        {
            _repositoryContext = context;
        }
        public Recipe Map(RecipeViewModel rvm)
        {
            Recipe r = new Recipe();
            r.IsCompleted = rvm.IsCompleted;
            r.Name = rvm.Name;
            r.NumberOfServing = rvm.NumberOfServing;
            r.PreparationTime = rvm.PreparationTime;
            r.Token = rvm.Token;
            r.Year = rvm.Year;            
            r.Direction = rvm.Direction;
            r.Description = rvm.Direction;
            r.CountryID = rvm.CountryID;            
            r.CategoryID = rvm.CategoryID;
            r.CaloriesPerServing = rvm.CaloriesPerServing;
            r.ID = rvm.ID;

            Country country = _repositoryContext.Country.FirstOrDefault(e => e.ID == rvm.CountryID);
            r.Country = country;

            Category category = _repositoryContext.Category.FirstOrDefault(e => e.ID == rvm.CategoryID);
            r.Category = category;

            foreach(IngredientViewModel item in rvm.Ingredients)
            {
                Ingredient ingredient = _repositoryContext.Ingredient.FirstOrDefault(e => e.ID == item.IngredientID);
                Unit unit = _repositoryContext.Unit.FirstOrDefault(e => e.ID == item.UnitID);
                RecipeIngredient RI = new RecipeIngredient()
                {
                    Ingredient = ingredient,
                    IngredientID = item.IngredientID,
                    Quantity = item.Quantity,
                    RecipeID = rvm.ID,
                    Unit = unit,
                    UnitID = item.UnitID
                };
                r.Ingredients.Add(RI);
            }            

            return r;
        }
        public  RecipeViewModel Map(Recipe r)
        {
            RecipeViewModel rvm = new RecipeViewModel();
            rvm.IsCompleted = r.IsCompleted;
            rvm.Name = r.Name;
            rvm.NumberOfServing = r.NumberOfServing;
            rvm.PreparationTime = r.PreparationTime;
            rvm.Token = r.Token;
            rvm.Year = r.Year;
            rvm.Direction = r.Direction;
            rvm.Description = r.Direction;
            rvm.CountryID = r.CountryID;
            rvm.CategoryID = r.CategoryID;
            rvm.CaloriesPerServing = r.CaloriesPerServing;
            rvm.ID = r.ID;            

            Country country = _repositoryContext.Country.FirstOrDefault(e => e.ID == r.CountryID);
            rvm.CountryID = country.ID;
            rvm.CountryName = country.Name;

            Category category = _repositoryContext.Category.FirstOrDefault(e => e.ID == r.CategoryID);
            rvm.CategoryID = category.ID;
            rvm.CategoryName = category.Name;

            rvm.Ingredients = new List<IngredientViewModel>();

            foreach (RecipeIngredient item in r.Ingredients)
            {

                IngredientViewModel temp = new IngredientViewModel() { 
                IngredientID = item.IngredientID,
                IngredientName = item.Ingredient.Name,
                Quantity =item.Quantity,
                UnitID = item.UnitID,
                UnitName = item.Unit.Name                
                };
                rvm.Ingredients.Add(temp);
            }

            return rvm;
        }

    }
}
