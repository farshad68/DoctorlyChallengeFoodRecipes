using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webservices.Models.Repository;

namespace Webservices.Models.DataManager
{
    public class RecipeTokenManager : IDataRepositoryTokenis<RecipeTokenLookUP>
    {
        readonly RepositoryContext _repositoryContext;
        public RecipeTokenManager(RepositoryContext context)
        {
            _repositoryContext = context;
        }
        public void Add(RecipeTokenLookUP entity)
        {
            _repositoryContext.RecipeTokenLookUP.Add(entity);
            _repositoryContext.SaveChanges();
        }              

        public RecipeTokenLookUP Get(Guid token)
        {
            var returnvalue = _repositoryContext.RecipeTokenLookUP
                 .FirstOrDefault(e => e.Token == token);            

            return returnvalue;
        }

        public RecipeTokenLookUP Get(long id)
        {
            var returnvalue = _repositoryContext.RecipeTokenLookUP
                 .FirstOrDefault(e => e.ID == id);

            return returnvalue;
        }                
    }
}