using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace GlimpseTest.Models
{
    public class Guitar
    {
        public int GuitarId { get; set; }

        public GuitarType Type { get; set; }

        [JsonIgnore]
        public virtual Manufacturer Manufacturer { get; set; }

        public string Name { get; set; }

        public string Material { get; set; }

        public decimal Price { get; set; }
    }
}