using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class UnitManager : IDataRepository<Unit>
    {
        readonly RepositoryContext _repositoryContext;
        public UnitManager(RepositoryContext context)
        {
            _repositoryContext = context;
        }


        public void Add(Unit entity)
        {
            _repositoryContext.Unit.Add(entity);
            _repositoryContext.SaveChanges();
        }

        public void Delete(Unit entity)
        {
            _repositoryContext.Unit.Remove(entity);
            _repositoryContext.SaveChanges();
        }

        public Unit Get(long id)
        {
            return _repositoryContext.Unit
                  .FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<Unit> GetAll()
        {
            return _repositoryContext.Unit.ToList();
        }

        public void Update(Unit dbEntity, Unit entity)
        {
            dbEntity.IsValid = entity.IsValid;
            dbEntity.Name = entity.Name;

            _repositoryContext.SaveChanges();
        }
        public bool Exist(Unit entity)
        {
            var q = from p in _repositoryContext.Unit
                    where _repositoryContext.Unit.Any(gi => gi.Equals(p))
                    select p;
            return q.Count() > 0;
        }
    }
}
