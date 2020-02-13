using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Webservices.Test
{
    public interface IMockDBHandler
    {
        DbContextOptions build();
        IMockDBHandler CategoryWithThreeMember();
        IMockDBHandler CountryWithThreeMember();
        IMockDBHandler IngredientWithThreeMember();
        Task<IMockDBHandler> ReciptWithThreeMemberAsync();
        IMockDBHandler UnitWithThreeMember();
    }
}