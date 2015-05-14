using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GlimpseTest.Models
{
    public class AlwaysDropAndCreateInitializer : DropCreateDatabaseAlways<GuitarShopContext>
    {
        protected override void Seed(GuitarShopContext context)
        {
            base.Seed(context);

            var yamaha = new Manufacturer
            {
                Name = "Yamaha",
                Models = new List<Guitar>
                {
                    new Guitar
                    {
                        Name = "C40",
                        Material = "Spruce",
                        Type = GuitarType.Classical
                    },
                    new Guitar
                    {
                        Name = "FS700S",
                        Material = "Rosewood",
                        Type = GuitarType.Acoustic
                    }
                }
            };

            var gibson = new Manufacturer
            {
                Name = "Gibson",
                Models = new List<Guitar>
                {
                    new Guitar
                    {
                        Name = "ES-175",
                        Material = "Maple",
                        Type = GuitarType.Electric
                    }
                }
            };

            context.Manufacturers.Add(yamaha);
            context.Manufacturers.Add(gibson);
        }
    }
}