using Blanquita_Inventarios.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(GenericVM viewModel)
        {
            return RedirectToAction("Home", "Home");        
        }

        public ActionResult OlvidastePassword()
        {
            return View();
        }

        public ActionResult ActualizarPassword()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }
    }
}