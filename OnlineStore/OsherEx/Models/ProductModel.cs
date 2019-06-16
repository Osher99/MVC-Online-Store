using OsherEx.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OsherEx.Models
{
    public class ProductModel
    {
        public int OwnerId { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("OwnerId")]
        public User User1 { get; set; }
        [ForeignKey("UserId")]
        public User User2 { get; set; }
        [Required(ErrorMessage = "Please enter your product name!"), DisplayName("Product Name:"), MinLength(5, ErrorMessage = "Product title must be at least 5 characters and no more then 20!"), MaxLength(20, ErrorMessage = "Product title must be at least 5 characters and no more then 20!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter your Short Description of the product!"), DisplayName("Short Description:"), MaxLength(500, ErrorMessage = "No more then 500 characters for Product Short Description")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Please enter your Long Description of the product!"), DisplayName("Long Description:"), MaxLength(500, ErrorMessage = "No more then 40000 characters for Product Short Description")]
        public string LongDescription { get; set; }
        public DateTime PublishedTime { get; set; }

        public DateTime? CartAddedTime { get; set; }
        public DateTime? SoldTime { get; set; }
        [Required(ErrorMessage = "Please enter the Price of the product!"), DisplayName("Price:"), DataType(DataType.Currency), RegularExpression(@"^\$?([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?)$", ErrorMessage = "{0} must be a Number.")]
        public double Price { get; set; }

        public byte[] Picture1 { get; set; }
        public byte[] Picture2 { get; set; }
        public byte[] Picture3 { get; set; }
        public State State { get; set; }

        public Category Category { get; set; }
    }
}