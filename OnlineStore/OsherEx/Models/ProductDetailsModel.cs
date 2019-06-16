using OsherEx.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsherEx.Models
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime PublishedTime { get; set; }
        public byte[] Picture1 { get; set; }
        public byte[] Picture2 { get; set; }
        public byte[] Picture3{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public double Price { get; set; }
        public DateTime BirthDate { get; set; }
        public State State { get; set; }

        public Category Category { get; set; }

    }
}