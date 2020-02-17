using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webservices.Models
{
    public class Unit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public override bool Equals(object value)
        {
            Unit that = value as Unit;

            return (that != null)
                && (this.ID == that.ID)
                && (this.Name == that.Name)
                && (this.IsValid == that.IsValid)
                ;
        }
    }
}
