using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class ArticulosPDAVM
    {
        public int FiltroIdConfiguracion { get; set; }
        public List<ControlDDL> FiltroInventarios { get; set; }

        public List<Listado_ArticulosPDA> Listado { get; set; }
    }
}