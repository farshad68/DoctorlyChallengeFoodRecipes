using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.RequestModel.Account
{
    public class RegisterRequestModel
    {
        [Required]        
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm  password")]
        [Compare("Password",ErrorMessage ="Password and Confirmationpassword do not match")]
        public string ConfirmPassword { get; set; }
    }
}
