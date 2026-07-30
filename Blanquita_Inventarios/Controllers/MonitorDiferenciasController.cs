using Blanquita_Inventarios.Models;
using Blanquita_Inventarios.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Controllers
{
    public class MonitorDiferenciasController : Controller
    {
        // GET: MonitorDiferencias
        public ActionResult Capturas()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica {Campo00 = "1", Campo01 = "1", Campo02 = "A001", Campo03 = "Artículo 001", Campo04 = "Conteo 1", Campo05 = "Luis Perez"},
                    new ListaGenerica {Campo00 = "2", Campo01 = "2", Campo02 = "A002", Campo03 = "Artículo 002", Campo04 = "Conteo 1", Campo05 = "Luis Perez"},
                    new ListaGenerica {Campo00 = "3", Campo01 = "3", Campo02 = "A003", Campo03 = "Artículo 003", Campo04 = "Conteo 2", Campo05 = "Luis Perez"},
                    new ListaGenerica {Campo00 = "4", Campo01 = "4", Campo02 = "A004", Campo03 = "Artículo 004", Campo04 = "Conteo 1", Campo05 = "Luis Perez"},
                    new ListaGenerica {Campo00 = "5", Campo01 = "5", Campo02 = "A005", Campo03 = "Artículo 005", Campo04 = "Conteo 2", Campo05 = "Luis Perez"},
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult Costos()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "A001", Campo01 = "Artículo 1", Campo02 = "Inactivo"},
                    new ListaGenerica { Campo00 = "A002", Campo01 = "Artículo 2", Campo02 = "Bloqueado por Almacén"},
                    new ListaGenerica { Campo00 = "A003", Campo01 = "Artículo 3", Campo02 = "No tienen Costo en el Almacén"},
                    new ListaGenerica { Campo00 = "A004", Campo01 = "Artículo 4", Campo02 = "Inactivo"},
                    new ListaGenerica { Campo00 = "A005", Campo01 = "Artículo 5", Campo02 = "No tienen Costo en el Almacén"},
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult MontosUno()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica>
            {
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "$3,500.00", Campo03 = "$3,200.00", Campo04 = "$300.00", Campo05 = "8.57%" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "$6,300.00", Campo03 = "$6,200.00", Campo04 = "$100.00", Campo05 = "1.58%" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "$4,800.00", Campo03 = "$3,800.00", Campo04 = "$1,000.00", Campo05 = "20.83%" }
            };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult MontosDos()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica>
            {
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "$3,500.00", Campo03 = "$3,200.00", Campo04 = "$300.00", Campo05 = "8.57%" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "$6,300.00", Campo03 = "$6,200.00", Campo04 = "$100.00", Campo05 = "1.58%" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "$4,800.00", Campo03 = "$3,800.00", Campo04 = "$1,000.00", Campo05 = "20.83%" }

            };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult ConteoUno()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica>
            {
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A001", Campo03 = "Artículo 1", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "3,000", Campo08 = "", Campo09 = "1" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A002", Campo03 = "Artículo 2", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "1,860", Campo08 = "", Campo09 = "4" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A003", Campo03 = "Artículo 3", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "4,150", Campo08 = "", Campo09 = "6" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A004", Campo03 = "Artículo 4", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "1,780", Campo08 = "", Campo09 = "8" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A005", Campo03 = "Artículo 5", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "569", Campo08 = "", Campo09 = "9" },
            };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult Reporte()
        {
            GenericVM viewModel = new GenericVM();
            List<ListaGenerica> listado = new List<ListaGenerica> {
                new ListaGenerica{ Campo00 = "125, 265, 236", Campo01 = "8411580441352", Campo02 = "KIT PARA INSTALACION DE FREGADERO", Campo03 = "4.00", Campo04 = "PZA", Campo05 = "$694.00", Campo06 = "$2,776.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "125", Campo01 = "0457514604801", Campo02 = "COLADERA REDONDA BLANCA", Campo03 = "10.00", Campo04 = "PZA", Campo05 = "$48.00", Campo06 = "$480.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "265, 365", Campo01 = "8504858764120", Campo02 = "VALVULA DE LLENADO", Campo03 = "21.00", Campo04 = "PZA", Campo05 = "$59.00", Campo06 = "$1,239.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "15, 88", Campo01 = "8043574055415", Campo02 = "COLADERA UNIVERSAL", Campo03 = "9.00", Campo04 = "PZA", Campo05 = "$35.00", Campo06 = "$315.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "102, 654", Campo01 = "8018257522470", Campo02 = "VALVULA DE LLENADO CON FLOTADOR", Campo03 = "13.00", Campo04 = "PZA", Campo05 = "$68.00", Campo06 = "$884.00", Campo07 = "" }
            };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult Ajustes()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica>
            {
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A001", Campo03 = "Artículo 1", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "3000", Campo08 = "", Campo09 = "1" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A002", Campo03 = "Artículo 2", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "1860", Campo08 = "", Campo09 = "4" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A003", Campo03 = "Artículo 3", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "4150", Campo08 = "", Campo09 = "6" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A004", Campo03 = "Artículo 4", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "1780", Campo08 = "", Campo09 = "8" },
                new ListaGenerica {Campo00 = "Almacén 1", Campo01 = "CONSTR Y OBRA", Campo02 = "A005", Campo03 = "Artículo 5", Campo04 = "PIEZA", Campo05 = "", Campo06 = "", Campo07 = "569", Campo08 = "", Campo09 = "9" },
            };

            viewModel.Listado = listado;

            return View(viewModel);
        }
    }
}