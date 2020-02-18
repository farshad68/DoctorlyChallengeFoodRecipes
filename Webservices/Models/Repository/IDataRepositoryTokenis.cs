using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.Models.Repository
{
    public interface IDataRepositoryTokenis<TEntity>
    {        
        TEntity Get(long id);
        TEntity Get(Guid token);
        void Add(TEntity entity);     
    }
}
