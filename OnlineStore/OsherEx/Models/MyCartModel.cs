using OsherEx.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsherEx.Models
{
    public class MyCartModel
    {
        public IEnumerable<Product> Products { get; set; }
        public double FullPrice { get; set; }

    }
}