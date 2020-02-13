using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class CategoryManager : IDataRepository<Category>
    {
        readonly RepositoryContext _repositoryContext;
        public CategoryManager(RepositoryContext context)
        {
            _repositoryContext = context;
        }

        
        public void Add(Category entity)
        {
            _repositoryContext.Category.Add(entity);
            _repositoryContext.SaveChanges();
        }

        public void Delete(Category entity)
        {
            _repositoryContext.Category.Remove(entity);
            _repositoryContext.SaveChanges();
        }

        public Category Get(long id)
        {
            return _repositoryContext.Category
                  .FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _repositoryContext.Category.ToList();
        }

        public void Update(Category dbEntity, Category entity)
        {
            dbEntity.IsValid = entity.IsValid;
            dbEntity.Name = entity.Name;           

            _repositoryContext.SaveChanges();
        }
    }
}
