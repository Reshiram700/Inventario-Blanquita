using Blanquita_Inventarios.Models;
using Blanquita_Inventarios.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            GenericVM viewModel = new GenericVM();
            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Numero00 = 1, Campo00 = "Pedro Torres", Campo01 = "pedro", Campo02 = "pedro@test.com", Campo03 = "Administrador del Sistema" },
                    new ListaGenerica { Numero00 = 2, Campo00 = "Ismael Garza", Campo01 = "islgz", Campo02 = "ismael@test.com", Campo03 = "Encargado del Inventario" },
                    new ListaGenerica { Numero00 = 3, Campo00 = "Paola Espinoza", Campo01 = "paola90", Campo02 = "paola@test.com", Campo03 = "Responsable de Levantamiento de Inventario" },
                    new ListaGenerica { Numero00 = 4, Campo00 = "Daniela Rodriguez", Campo01 = "dani00", Campo02 = "daniela@test.com", Campo03 = "Responsable de Levantamiento de Inventario" },
                    new ListaGenerica { Numero00 = 5, Campo00 = "Luis Gutierrez", Campo01 = "luis", Campo02 = "luis@test.com", Campo03 = "Super Usuario" },
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(GenericVM viewModel)
        {
            return View(viewModel);
        }

        public ActionResult Create()
        {
            GenericVM viewModel = new GenericVM();            

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GenericVM viewModel)
        {
            return RedirectToAction("Index", "Usuarios");
        }

        public ActionResult Details(int? id)
        {
            GenericVM viewModel = new GenericVM();
            viewModel.Campo00 = "Pedro Torres";
            viewModel.Campo01 = "pedro";
            viewModel.Campo02 = "ptorres122";
            viewModel.Campo03 = "pedro@test.com";


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(GenericVM viewModel)
        {
            return RedirectToAction("Index", "Usuarios");
        }
    }
}