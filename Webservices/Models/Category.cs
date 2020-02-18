using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Webservices.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [MaxLength(30)]
        [Column(TypeName= "nvarchar(30)")]    
        [MinLength(3)]
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        public override bool Equals(object value)
        {
            Category that = value as Category;
            
            return (that != null)
                && (this.ID == that.ID)
                && (this.Name == that.Name)
                && (this.IsValid == that.IsValid)
                ;
        }
        
    }
}
