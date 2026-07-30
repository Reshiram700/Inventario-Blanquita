using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Diferencias_CapturasVM
    {
        public int FiltroIdConfiguracion { get; set; }
        public List<ControlDDL> FiltroInventarios { get; set; }
        public int FiltroMarbete { get; set; }
        public int FiltroIdConteo { get; set; }
        public List<ControlDDL> FiltroConteos { get; set; }
        public string FiltroCodigoArticulo { get; set; }
        public Tabla_ResultadoCapturas ResultadoCapturas { get; set; }
    }
}