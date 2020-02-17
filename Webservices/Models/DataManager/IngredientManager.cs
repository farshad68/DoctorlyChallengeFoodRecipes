using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class IngredientManager : IDataRepository<Ingredient>
    {
        readonly RepositoryContext _repositoryContext;
        public IngredientManager(RepositoryContext context)
        {
            _repositoryContext = context;
        }


        public void Add(Ingredient entity)
        {
            _repositoryContext.Ingredient.Add(entity);
            _repositoryContext.SaveChanges();
        }

        public void Delete(Ingredient entity)
        {
            _repositoryContext.Ingredient.Remove(entity);
            _repositoryContext.SaveChanges();
        }

        public Ingredient Get(long id)
        {
            return _repositoryContext.Ingredient
                  .FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _repositoryContext.Ingredient.ToList();
        }

        public void Update(Ingredient dbEntity, Ingredient entity)
        {
            dbEntity.IsValid = entity.IsValid;
            dbEntity.Name = entity.Name;

            _repositoryContext.SaveChanges();
        }
        public bool Exist(Ingredient entity)
        {
            var q = from p in _repositoryContext.Ingredient
                    where _repositoryContext.Ingredient.Any(gi => gi.Equals(p))
                    select p;
            return q.Count() > 0;
        }
    }
}
