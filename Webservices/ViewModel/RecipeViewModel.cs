using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models;

namespace Webservices.ViewModel
{
    public class RecipeViewModel
    {
        public long ID { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }        
        public int Year { get; set; }
        public long PreparationTime { get; set; }
        public int NumberOfServing { get; set; }
        public float CaloriesPerServing { get; set; }
        public List<IngredientViewModel> Ingredients { get; set; }
        public bool IsCompleted { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }        

        public override bool Equals(object value)
        {
            RecipeViewModel that = value as RecipeViewModel;
            
            return (that != null)
                && (this.ID == that.ID)
                && (this.Name == that.Name)
                && (this.Description == that.Description)
                && (this.Direction == that.Direction)
                && (this.CountryID == that.CountryID)
                && (this.CountryName == that.CountryName)
                && (this.Year == that.Year)
                && (this.PreparationTime == that.PreparationTime)
                && (this.NumberOfServing == that.NumberOfServing)
                && (this.CaloriesPerServing == that.CaloriesPerServing)
                && (this.IsCompleted == that.IsCompleted)
                && (this.CategoryID == that.CategoryID)
                && (this.CategoryName == that.CategoryName)
                && this.Ingredients.EqualsOtherList(that.Ingredients)
                ;
        }
    }
}
