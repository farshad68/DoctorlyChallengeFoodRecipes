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

        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [MinLength(3)]
        public string Name { get; set; }
        public string Description { get; set; }
        [MinLength(10)]
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
            Recipe that = value as Recipe;

            return (that != null)
                && (this.ID == that.ID)
                && (this.Name == that.Name)
                && (this.Description == that.Description)
                && (this.Direction == that.Direction)
                && (this.CountryID == that.CountryID)
                && (this.Year == that.Year)
                && (this.PreparationTime == that.PreparationTime)
                && (this.NumberOfServing == that.NumberOfServing)
                && (this.CaloriesPerServing == that.CaloriesPerServing)
                && (this.IsCompleted == that.IsCompleted)
                && (this.CategoryID == that.CategoryID)
                && (this.Ingredients.Count == that.Ingredients.Count)
                && (this.Ingredients.EqualsOtherCollection(that.Ingredients));
        }
        public  bool EqualsAndIDdoesntmatter(object value)
        {
            Recipe that = value as Recipe;

            return (that != null)

                && (this.Name == that.Name)
                && (this.Description == that.Description)
                && (this.Direction == that.Direction)
                && (this.CountryID == that.CountryID)
                && (this.Year == that.Year)
                && (this.PreparationTime == that.PreparationTime)
                && (this.NumberOfServing == that.NumberOfServing)
                && (this.CaloriesPerServing == that.CaloriesPerServing)
                && (this.IsCompleted == that.IsCompleted)
                && (this.CategoryID == that.CategoryID)
                && (this.Ingredients.Count == that.Ingredients.Count)
                && (this.Ingredients.EqualsOtherCollection(that.Ingredients));
        }
    }
}

