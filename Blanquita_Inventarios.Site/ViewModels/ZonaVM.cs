using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class ZonaVM
    {
        public int IdZona { get; set; }
        public int IdConfiguracion { get; set; }
        public string Zona {  get; set; }
        public int MarbeteInicial { get; set; }
        public bool HabilitarMarbeteInicial { get; set; }
        public int MarbeteFinal { get; set; }
    }
}