using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class RecipeManager : IDataRepository<Recipe>
    {
        public void Add(Recipe entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Recipe entity)
        {
            throw new NotImplementedException();
        }

        public Recipe Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Recipe> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Recipe dbEntity, Recipe entity)
        {
            throw new NotImplementedException();
        }
    }
}
