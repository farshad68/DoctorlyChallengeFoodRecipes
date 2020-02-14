using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webservices.Mapper;
using System.Linq;
using Xunit;
using Webservices.ViewModel;

namespace Webservices.Test
{
    public class CustomMapperTest
    {
        private ICustomMapper _custumMapper;
        public CustomMapperTest()
        {

        }

        [Fact]
        public void Test_map_From_recipe_To_recipeVieModel()
        {
            DbContextOptions options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                _custumMapper = new CustomMapper(context);
                var acctual = context.Recipe.Include(y => y.Category)
                                            .Include(r => r.Country)
                                            .Include(w => w.Ingredients)
                                            .FirstOrDefaultAsync().Result;
                acctual.Ingredients = context.RecipeIngredient.Include(x => x.Ingredient).Include(y => y.Unit).Where(T => T.RecipeID == acctual.ID).ToList();

                //Act
                var recipDes = _custumMapper.Map(acctual);
                var expected = _custumMapper.Map(recipDes);

                // Assert  
                Assert.True(expected.Equals(acctual));
            }
        }
        [Fact]
        public void Test_map_From_recipeVieModel_To_recipe()
        {
            RecipeViewModel acctual =new MockDBHandler().buildMockRecipeView();
            DbContextOptions options = new MockDBHandler().CategoryWithThreeMember().CountryWithThreeMember().UnitWithThreeMember().IngredientWithThreeMember().ReciptWithThreeMember().build();

            using (var context = new RepositoryContext(options))
            {
                _custumMapper = new CustomMapper(context);

                //Act
                var recipDes = _custumMapper.Map(acctual);
                var expected = _custumMapper.Map(recipDes);

                // Assert  
                Assert.True(expected.Equals(acctual));
            }
        }
        
    }
}
