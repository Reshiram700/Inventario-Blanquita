using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_Reporte
    {
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public List<Listado_DiferenciasReporte> Listado {  get; set; }
    }
}