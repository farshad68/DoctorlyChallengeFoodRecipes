using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webservices.Data;

namespace Webservices.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public Guid Token { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public long CountryID { get; set; }
        public Country Country { get; set; }
        public int Year { get; set; }
        public long PreparationTime { get; set; }
        public int NumberOfServing { get; set; }
        public float CaloriesPerServing { get; set; }
        public ICollection<RecipeIngredient> Ingredients { get; set; }
        public bool IsCompleted { get; set; }
        public long CategoryID { get; set; }
        public Category Category { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public override bool Equals(object value)
        {
            Recipe secondRecipe = value as Recipe;

            return (secondRecipe != null)
                && (ID == secondRecipe.ID)
                && (Token == secondRecipe.Token)
                && (Name == secondRecipe.Name)
                && (Description == secondRecipe.Description)
                && (Direction == secondRecipe.Direction)
                && (CountryID == secondRecipe.CountryID)
                && (Year == secondRecipe.Year)
                && (PreparationTime == secondRecipe.PreparationTime)
                && (NumberOfServing == secondRecipe.NumberOfServing)
                && (CaloriesPerServing == secondRecipe.CaloriesPerServing)
                && (IsCompleted == secondRecipe.IsCompleted)
                && (CategoryID == secondRecipe.CategoryID)
                && (Ingredients.Count == secondRecipe.Ingredients.Count);
        }
    }
}
