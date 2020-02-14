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
        public Guid Token { get; set; }
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
            RecipeViewModel secondRVM = value as RecipeViewModel;
            
            return (secondRVM != null)
                && (ID == secondRVM.ID)
                && (Token == secondRVM.Token)
                && (Name == secondRVM.Name)
                && (Description == secondRVM.Description)
                && (Direction == secondRVM.Direction)
                && (CountryID == secondRVM.CountryID)
                && (CountryName == secondRVM.CountryName)
                && (Year == secondRVM.Year)
                && (PreparationTime == secondRVM.PreparationTime)
                && (NumberOfServing == secondRVM.NumberOfServing)
                && (CaloriesPerServing == secondRVM.CaloriesPerServing)
                && (IsCompleted == secondRVM.IsCompleted)
                && (CategoryID == secondRVM.CategoryID)
                && (CategoryName == secondRVM.CategoryName)
                && Ingredients.EqualsOtherList(secondRVM.Ingredients)
                ;
        }
    }
}
