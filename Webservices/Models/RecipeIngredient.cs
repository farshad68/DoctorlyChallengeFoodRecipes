using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.Models
{
    public class RecipeIngredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
        public int IngredientID { get; set; }
        public Ingredient Ingredient { get; set; }
        public float Quantity { get; set; }
        public int UnitID { get; set; }
        public Unit Unit { get; set; }
    }
}
