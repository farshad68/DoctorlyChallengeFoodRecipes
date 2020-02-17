using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webservices.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        [MinLength(2)]
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public override bool Equals(object value)
        {
            Country that = value as Country;

            return (that != null)
                && (this.ID == that.ID)
                && (this.Name == that.Name)
                && (this.IsValid == that.IsValid)
                ;
        }
    }
}
