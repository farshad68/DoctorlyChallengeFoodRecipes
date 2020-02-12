using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webservices.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public Guid Token { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public long CountryID { get; set; }
        public Country Country { get; set; }
        public int Year { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public int NumberOfServing { get; set; }
        public float CaloriesPerServing { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public bool IsCompleted { get; set; }        

    }
}
