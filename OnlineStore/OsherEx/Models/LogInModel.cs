using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OsherEx.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Please enter your User Name!"), DisplayName("User Name:"), StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Please enter your Password!"), DisplayName("Password:"), RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed."), StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}