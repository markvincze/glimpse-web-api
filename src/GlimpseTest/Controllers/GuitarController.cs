using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GlimpseTest.Models;

namespace GlimpseTest.Controllers
{
    public class GuitarController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var ctx = new GuitarShopContext())
            {
                var manufacturers = await ctx.Manufacturers.Include("Models").ToListAsync();

                await this.LoadPrices(manufacturers.SelectMany(m => m.Models));

                return this.View(manufacturers);
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