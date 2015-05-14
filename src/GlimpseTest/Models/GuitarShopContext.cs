using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GlimpseTest.Models
{
    public class GuitarShopContext : DbContext
    {
        public GuitarShopContext() : base("GuitarShopContext")
        {
        }

        public DbSet<Guitar> Guitars { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }
    }
}