using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GlimpseTest.Models;
using Glimpse.Core;

namespace GlimpseTest.Controllers
{
    public class GuitarApiController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            using (var ctx = new GuitarShopContext())
            {
                Trace.Write("Loading manufacturers from the database.");

                List<Manufacturer> manufacturers;
                using (GlimpseTimeline.Capture("Loading from DB"))
                {
                    manufacturers = await ctx.Manufacturers.Include("Models").ToListAsync();
                }

                Trace.Write("Getting price information");

                using (GlimpseTimeline.Capture("Fetching product prices"))
                {
                    await this.LoadPrices(manufacturers.SelectMany(m => m.Models));
                }

                return this.Ok(manufacturers);
            }
        }

        private async Task LoadPrices(IEnumerable<Guitar> guitars)
        {
            Random rnd = new Random();
            foreach (var guitar in guitars)
            {
                guitar.Price = rnd.Next(100, 500);
            }

            await Task.Delay(500);
        }
    }
}