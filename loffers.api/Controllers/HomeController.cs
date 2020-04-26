using Loffers.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace loffers.api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [Route("offer/{id}")]
        public async Task<ActionResult> OfferDetails(string id, string unit)
        {
            var service = await new OffersService().Details(id, 0, 0, unit);
            return View(service);
        }
    }
}
