using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
