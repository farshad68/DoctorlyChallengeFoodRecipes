using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Webservices.Test
{
    public interface IMockDBHandler
    {
        DbContextOptions<RepositoryContext> build();
        IMockDBHandler CategoryWithThreeMember();
        IMockDBHandler CountryWithThreeMember();
        IMockDBHandler IngredientWithThreeMember();
        IMockDBHandler ReciptWithThreeMember();
        IMockDBHandler UnitWithThreeMember();
    }
}