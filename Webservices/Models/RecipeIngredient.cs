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
        public long RecipeID { get; set; }
        public Recipe Recipe { get; set; }
        public long IngredientID { get; set; }
        public Ingredient Ingredient { get; set; }
        public float Quantity { get; set; }
        public long UnitID { get; set; }
        public Unit Unit { get; set; }        
    }
}
