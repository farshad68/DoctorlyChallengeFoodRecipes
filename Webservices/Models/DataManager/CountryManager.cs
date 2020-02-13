using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class CountryManager : IDataRepository<Country>
    {
        readonly RepositoryContext _repositoryContext;
        public CountryManager(RepositoryContext context)
        {
            _repositoryContext = context;
        }


        public void Add(Country entity)
        {
            _repositoryContext.Country.Add(entity);
            _repositoryContext.SaveChanges();
        }

        public void Delete(Country entity)
        {
            _repositoryContext.Country.Remove(entity);
            _repositoryContext.SaveChanges();
        }

        public Country Get(long id)
        {
            return _repositoryContext.Country
                  .FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<Country> GetAll()
        {
            return _repositoryContext.Country.ToList();
        }

        public void Update(Country dbEntity, Country entity)
        {
            dbEntity.IsValid = entity.IsValid;
            dbEntity.Name = entity.Name;

            _repositoryContext.SaveChanges();
        }
    }
}
