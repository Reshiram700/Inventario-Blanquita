using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class AjustesVM
    {
        public List<ControlDDL> FiltroInventarios { get; set; }
        public int FiltroIdConfiguracion { get; set; }
    }
}