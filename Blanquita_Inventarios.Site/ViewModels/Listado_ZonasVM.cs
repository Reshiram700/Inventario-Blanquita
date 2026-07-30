using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Listado_ZonasVM
    {
        public int IdConfiguracion { get; set; }
        public string FiltroFechas {  get; set; }
        public string FiltroZona { get; set; }
        
        public List<Listado_Zonas> Listado {  get; set; }
    }
}