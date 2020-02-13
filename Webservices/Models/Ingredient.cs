﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webservices.Models
{
    public class Ingredient
    {          
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public ICollection<RecipeIngredient> Recipes { get; set; }
    }
}
