using Blanquita_Inventarios.Models;
using Blanquita_Inventarios.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Controllers
{
    public class ConfiguracionesController : Controller
    {
        // GET: Configuraciones
        public ActionResult Index()
        {
            GenericVM viewModel = new GenericVM();
            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Numero00 = 1, Campo00 = "01/01/2025 00:00", Campo01 = "Inventario 01", Campo02 = "105.259.25.1", Campo03 = "Instancia 01", Campo04 = "administrador", Campo05 = "123456", Campo06 = "BDCentral", Campo07 = "Almacen 01" },
                    new ListaGenerica { Numero00 = 2, Campo00 = "01/02/2025 00:00", Campo01 = "Inventario 02", Campo02 = "192.36.52.1", Campo03 = "Instancia 01", Campo04 = "administrador", Campo05 = "123456", Campo06 = "BDInventarioNorte", Campo07 = "Almacen Norte" },
                    new ListaGenerica { Numero00 = 3, Campo00 = "01/03/2025 00:00", Campo01 = "Inventario 03", Campo02 = "130.369.2.14", Campo03 = "Instancia 01", Campo04 = "administrador", Campo05 = "123456", Campo06 = "BDSur", Campo07 = "Almacen Sur" },
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
            return RedirectToAction("Index", "Configuraciones");
        }

        public ActionResult Details(int? id)
        {
            GenericVM viewModel = new GenericVM();
            viewModel.Numero00 = 1;
            viewModel.Campo00 = "01/01/2025 00:00";
            viewModel.Campo01 = "Inventario 01";
            viewModel.Campo02 = "105.259.25.1";
            viewModel.Campo03 = "Instancia 01";
            viewModel.Campo04 = "administrador";
            viewModel.Campo05 = "123456";
            viewModel.Campo06 = "BDCentral";
            viewModel.Campo07 = "Almacen 01";


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(GenericVM viewModel)
        {
            return RedirectToAction("Index", "Configuraciones");
        }

        public ActionResult Zonas()
        {
            GenericVM viewModel = new GenericVM();
            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Numero00 = 1, Campo00 = "01/02/2025 00:00", Campo01 = "Zona Centro", Campo02 = "1", Campo03 = "5"},
                    new ListaGenerica { Numero00 = 1, Campo00 = "02/02/2025 00:00", Campo01 = "Zona Sur", Campo02 = "6", Campo03 = "10"},
                    new ListaGenerica { Numero00 = 1, Campo00 = "03/02/2025 00:00", Campo01 = "Zona Norte", Campo02 = "11", Campo03 = "15"}
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }


        public ActionResult CreateZona()
        {
            GenericVM viewModel = new GenericVM();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateZona(GenericVM viewModel)
        {
            return RedirectToAction("Index", "Configuraciones");
        }

        public ActionResult GenerarCodes()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "z217dc", Campo01 = "01/01/2025 00:00" , Campo02 = "Pedro Torres", Campo03 = "", Campo04 = "", Campo05 = "Por Utilizar"},
                    new ListaGenerica { Campo00 = "aw528e", Campo01 = "01/02/2025 00:00" , Campo02 = "Pedro Torres", Campo03 = "", Campo04 = "", Campo05 = "Por Utilizar"},
                    new ListaGenerica { Campo00 = "rvb4q1", Campo01 = "01/03/2025 00:00" , Campo02 = "Pedro Torres", Campo03 = "", Campo04 = "", Campo05 = "Por Utilizar"},
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult FormatoUno()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10000"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10001"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10002"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10003"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10004"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10005"}
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult FormatoDos()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10000"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10001"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10002"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10003"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10004"},
                    new ListaGenerica { Campo00 = "Almacén 1", Campo02 = "10005"}
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }
    }
}
