﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.ViewModel
{
    public class IngredientViewModel
    {
        public string IngredientName { get; set; }
        public float Quantity { get; set; }
        public string UnitName { get; set; }
        public long IngredientID { get; set; }
        public long UnitID { get; set; }
    }
}