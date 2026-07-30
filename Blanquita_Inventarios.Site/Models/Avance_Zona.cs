using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Avance_Zona
    {
        public decimal Avance {  get; set; }
        public decimal Monto { get; set; }
        public string Tiempo { get; set; }
        public string Inventario { get; set; }
        public string Almacen { get; set; }
        public List<Listado_AvanceZona> Listado {  get; set; }
    }
}