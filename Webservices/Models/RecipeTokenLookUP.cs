using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.Models
{
    public class RecipeTokenLookUP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public Guid Token { get; set; }
        public long RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}
