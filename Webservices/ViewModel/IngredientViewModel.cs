using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.ViewModel
{
    public class IngredientViewModel
    {
        public long IngredientID { get; set; }
        public string IngredientName { get; set; }
        public float Quantity { get; set; }
        public long UnitID { get; set; }
        public string UnitName { get; set; }        
     
        public override bool Equals(object value)
        {
            IngredientViewModel secondIVM = value as IngredientViewModel;

            return (secondIVM != null)
                && (IngredientID == secondIVM.IngredientID)
                && (IngredientName == secondIVM.IngredientName)
                && (Quantity == secondIVM.Quantity)
                && (UnitID == secondIVM.UnitID)
                && (UnitName == secondIVM.UnitName);
        }
    }
}
