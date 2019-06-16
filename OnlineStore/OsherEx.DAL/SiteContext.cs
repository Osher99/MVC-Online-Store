using OsherEx.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsherEx.DAL
{
    class SiteContext : DbContext
    {
        public SiteContext() : base("myNewConnection") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
