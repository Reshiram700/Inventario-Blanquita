using Blanquita_Inventarios.Models;
using Blanquita_Inventarios.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Controllers
{
    public class PdaController : Controller
    {
        // GET: Pda
        public ActionResult Index()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica>
            {
                new ListaGenerica { Campo00 = "PDA 001", Campo01 = "180"},
                new ListaGenerica { Campo00 = "PDA 002", Campo01 = "460"},
                new ListaGenerica { Campo00 = "PDA 003", Campo01 = "250"},
                new ListaGenerica { Campo00 = "PDA 004", Campo01 = "374"},
            };

            viewModel.Listado = listado;

            return View(viewModel);
        }
    }
}
