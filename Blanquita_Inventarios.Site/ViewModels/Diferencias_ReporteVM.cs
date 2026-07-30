using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Diferencias_ReporteVM
    {
        public string Categoria { get; set; }
        public string Almacen {  get; set; }
        public string Servidor { get; set; }

        public List<Diferencias_Zonas> ListadoZonas { get; set; }
        public List<Diferencias_Reporte> Listado {  get; set; }
    }
}