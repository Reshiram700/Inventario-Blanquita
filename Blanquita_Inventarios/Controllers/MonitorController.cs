using Blanquita_Inventarios.Models;
using Blanquita_Inventarios.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Controllers
{
    public class MonitorController : Controller
    {
        // GET: Monitor
        public ActionResult Index()
        {
            GenericVM viewModel = new GenericVM();
            
            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "Almacen 1", Campo01 = "Zona Centro", Campo02 = "60%", Campo03 = "82", Campo04 = "1:22", Campo05 = "60", Campo06 = "100", Campo07 = "40", Campo08 = "6000"  },
                    new ListaGenerica { Campo00 = "Almacen 2", Campo01 = "Zona Sur", Campo02 = "20%", Campo03 = "35", Campo04 = "0:35", Campo05 = "20", Campo06 = "100", Campo07 = "80", Campo08 = "2000"  },
                    new ListaGenerica { Campo00 = "Almacen 3", Campo01 = "Zona Norte", Campo02 = "52%", Campo03 = "43", Campo04 = "0:43", Campo05 = "52", Campo06 = "100", Campo07 = "48", Campo08 = "5200"  },
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult Marbetes()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "Almacen 1", Campo01 = "Zona Centro", Campo02 = "Marbete 1", Campo03 = "Abierto", Campo04 = "3,200", Campo05 = "1:00", Campo06 = "Luis Perez"  },
                    new ListaGenerica { Campo00 = "Almacen 1", Campo01 = "Zona Centro", Campo02 = "Marbete 2", Campo03 = "Abierto", Campo04 = "1,260", Campo05 = "0:20", Campo06 = "Luis Perez"  },
                    new ListaGenerica { Campo00 = "Almacen 1", Campo01 = "Zona Centro", Campo02 = "Marbete 3", Campo03 = "Cerrado", Campo04 = "2,310", Campo05 = "0:52", Campo06 = "Francisco Torres"  },
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult Detalle()
        {
            GenericVM viewModel = new GenericVM();

            List<ListaGenerica> listado = new List<ListaGenerica> {
                    new ListaGenerica { Campo00 = "1", Campo01 = "Almacen 1", Campo02 = "1", Campo03 = "4538704", Campo04 = "A001", Campo05 = "Producto 1", Campo06 = "45", Campo07 = "PIEZAS", Campo08 = "01/02/2025 14:25", Campo09 = "Luis Perez" },
                    new ListaGenerica { Campo00 = "2", Campo01 = "Almacen 1", Campo02 = "2", Campo03 = "5936217", Campo04 = "A002", Campo05 = "Producto 2", Campo06 = "37", Campo07 = "PIEZAS", Campo08 = "01/02/2025 14:28", Campo09 = "Luis Perez" },
                    new ListaGenerica { Campo00 = "3", Campo01 = "Almacen 1", Campo02 = "3", Campo03 = "0254193", Campo04 = "A003", Campo05 = "Producto 3", Campo06 = "96", Campo07 = "PIEZAS", Campo08 = "01/02/2025 14:32", Campo09 = "Luis Perez" },
                    new ListaGenerica { Campo00 = "4", Campo01 = "Almacen 1", Campo02 = "4", Campo03 = "2561007", Campo04 = "A004", Campo05 = "Producto 4", Campo06 = "24", Campo07 = "PIEZAS", Campo08 = "01/02/2025 14:45", Campo09 = "Luis Perez" },
                    new ListaGenerica { Campo00 = "5", Campo01 = "Almacen 1", Campo02 = "5", Campo03 = "4185047", Campo04 = "A005", Campo05 = "Producto 5", Campo06 = "84", Campo07 = "PIEZAS", Campo08 = "01/02/2025 14:51", Campo09 = "Luis Perez" }
                };

            viewModel.Listado = listado;

            return View(viewModel);
        }

        public ActionResult Reporte()
        {
            GenericVM viewModel = new GenericVM();
            List<ListaGenerica> listado = new List<ListaGenerica> { 
                new ListaGenerica{ Campo00 = "75542841", Campo01 = "8411580441352", Campo02 = "KIT PARA INSTALACION DE FREGADERO", Campo03 = "4.00", Campo04 = "PZA", Campo05 = "$694.00", Campo06 = "$2,776.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "12504780", Campo01 = "0457514604801", Campo02 = "COLADERA REDONDA BLANCA", Campo03 = "10.00", Campo04 = "PZA", Campo05 = "$48.00", Campo06 = "$480.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "71368085", Campo01 = "8504858764120", Campo02 = "VALVULA DE LLENADO", Campo03 = "21.00", Campo04 = "PZA", Campo05 = "$59.00", Campo06 = "$1,239.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "40872058", Campo01 = "8043574055415", Campo02 = "COLADERA UNIVERSAL", Campo03 = "9.00", Campo04 = "PZA", Campo05 = "$35.00", Campo06 = "$315.00", Campo07 = "" },
                new ListaGenerica{ Campo00 = "58011470", Campo01 = "8018257522470", Campo02 = "VALVULA DE LLENADO CON FLOTADOR", Campo03 = "13.00", Campo04 = "PZA", Campo05 = "$68.00", Campo06 = "$884.00", Campo07 = "" }
            };

            viewModel.Listado = listado;

            return View(viewModel);
        }
    }
}