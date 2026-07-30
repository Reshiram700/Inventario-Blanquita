using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_Zonas
    {
        public int IdZona { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Zona { get; set; }
        public int MarbeteInicial { get; set; }
        public int MarbeteFinal { get; set; }
        public bool MarbetesCreados { get; set; }
    }
}