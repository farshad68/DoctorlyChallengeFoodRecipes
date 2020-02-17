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
            IngredientViewModel that = value as IngredientViewModel;

            return (that != null)
                && (this.IngredientID == that.IngredientID)
                && (this.IngredientName == that.IngredientName)
                && (this.Quantity == that.Quantity)
                && (this.UnitID == that.UnitID)
                && (this.UnitName == that.UnitName);
        }
    }
}
