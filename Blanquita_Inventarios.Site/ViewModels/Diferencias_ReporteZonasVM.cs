using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Diferencias_ReporteZonasVM
    {
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Almacen { get; set; }
        public string Servidor { get; set; }
        public List<string> ListaCategorias { get; set; }
        public List<Diferencias_Reporte> Listado { get; set; }
    }
}