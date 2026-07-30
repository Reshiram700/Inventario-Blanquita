using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Avance_DetalleMarbete
    {
        public int IdConfiguracion { get; set; }
        public string Inventario { get; set; }
        public string Almacen {  get; set; }
        public int IdMarbete { get; set; }
        public int Marbete { get; set; }
        public List<Listado_AvanceDetalleMarbete> Listado {  get; set; }
    }
}