using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Listado_ConfiguracionesVM
    {
        public string FiltroFechas {  get; set; }
        public string FiltroInventario { get; set; }
        public string FiltroServidor { get; set; }
        public List<Listado_Configuraciones> Listado {  get; set; }
    }
}