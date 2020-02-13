using Webservices.Models;
using Webservices.ViewModel;

namespace Webservices.Mapper
{
    public interface ICustomMapper
    {
        RecipeViewModel Map(Recipe r);
        Recipe Map(RecipeViewModel rvm);
    }
}