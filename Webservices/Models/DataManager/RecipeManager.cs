using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class RecipeManager : IDataRepository<Recipe>
    {
        readonly RepositoryContext _repositoryContext;
        public RecipeManager(RepositoryContext context)
        {
            _repositoryContext = context;
        }
        public void Add(Recipe entity)
        {
            _repositoryContext.Recipe.Add(entity);
            _repositoryContext.SaveChanges();
        }

        public void Delete(Recipe entity)
        {
            _repositoryContext.Recipe.Remove(entity);
            _repositoryContext.SaveChanges();
        }

        public Recipe Get(long id)
        {
            var returnvalue = _repositoryContext.Recipe
                 .FirstOrDefault(e => e.ID == id);

            returnvalue.Ingredients = _repositoryContext.RecipeIngredient.Include(x => x.Ingredient).Include(y => y.Unit).Where(T => T.RecipeID == id).ToList();
            
            return returnvalue;
        }

        public IEnumerable<Recipe> GetAll()
        {            
            var returnlist = _repositoryContext.Recipe.ToList();
            
            foreach (var item in returnlist)
            {
                item.Ingredients = _repositoryContext.RecipeIngredient.Include(x => x.Ingredient).Include(y => y.Unit).Where(T => T.RecipeID == item.ID).ToList();
            }
            return returnlist;
        }

        public void Update(Recipe dbEntity, Recipe entity)
        {
            dbEntity.CaloriesPerServing = entity.CaloriesPerServing;
            dbEntity.Category = entity.Category;
            dbEntity.Country = entity.Country;
            dbEntity.Description = entity.Description;
            dbEntity.Direction = entity.Direction;
            dbEntity.Ingredients = entity.Ingredients;
            dbEntity.IsCompleted = entity.IsCompleted;
            dbEntity.Name = entity.Name;
            dbEntity.NumberOfServing = entity.NumberOfServing;
            dbEntity.PreparationTime = entity.PreparationTime;
            dbEntity.Token = entity.Token;
            dbEntity.Year = entity.Year;

            _repositoryContext.SaveChanges();
        }
    }
}
