using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Reporte01VM
    {
        public string Inventario {  get; set; }        
        public string Almacen {  get; set; }
        public int Marbete { get; set; }
        public string Capturo { get; set; }
        public string Servidor { get; set; }
        public List<DatosBarcode> Listado { get; set; }

    }
}