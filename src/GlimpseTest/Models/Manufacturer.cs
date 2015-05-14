using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlimpseTest.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }

        public string Name { get; set; }

        public virtual IList<Guitar> Models { get; set; }
    }
}
