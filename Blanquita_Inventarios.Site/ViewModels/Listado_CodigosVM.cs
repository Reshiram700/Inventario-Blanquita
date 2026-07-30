using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Listado_CodigosVM
    {
        public int IdConfiguracion { get; set; }
        public List<Listado_Codigos> Listado { get; set; }
    }
}