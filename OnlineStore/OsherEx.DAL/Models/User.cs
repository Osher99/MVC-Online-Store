using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsherEx.DAL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your first name!"), DisplayName("First Name:"), MaxLength(25),RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your last name!"), DisplayName("Last Name:"), MaxLength(25), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }
        [DataType(DataType.Date), Required(ErrorMessage = "Please enter your Birth Day address!"), DisplayName("Day of Birth:"), Range(typeof(DateTime), "1/1/1930", "1/1/2019",
        ErrorMessage = "Value for {0} must be between {1} and {2}"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.EmailAddress), Required(ErrorMessage = "Please enter your Email address!"), DisplayName("Email Address:"), StringLength(50, ErrorMessage = "Max 50 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your User Name!"), DisplayName("User Name:"), StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Please enter your Password!"), DisplayName("Password:"), StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password), DisplayName("Repeat Password:"), StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ConfirmPassowrd { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

    }
}
