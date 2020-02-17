using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models;
using Microsoft.EntityFrameworkCore;

namespace Webservices
{
    public static class Extentions
    {

        public static IQueryable<Recipe> CompleteRecipe(this RepositoryContext context)
        {
            return context.Recipe
                .Include(r => r.Ingredients);
                
        }
        public static bool EqualsOtherList<T>(this List<T> thisList, List<T> theOtherList)
        {
            if (thisList == null || theOtherList == null ||
                thisList.Count != theOtherList.Count) return false;
            return !thisList.Where((t, i) => !t.Equals(theOtherList[i])).Any();
        }
        public static bool EqualsOtherCollection<T>(this ICollection<T> thisCollection, ICollection<T> theOtherCollection)
        {
            return EqualsOtherList<T>(thisCollection.ToList(), theOtherCollection.ToList());
        }
    }
}
