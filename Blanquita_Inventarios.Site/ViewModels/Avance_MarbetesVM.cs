using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Avance_MarbetesVM
    {
        public int FiltroIdConfiguracion { get; set; }
        public List<ControlDDL> FiltroInventarios { get; set; }
        public int FiltroIdZona { get; set; }
        public List<ControlDDL> FiltroZonas {get; set;}
        public int FiltroMarbete {  get; set; }
        public Avance_Marbetes Avance {  get; set; }
    }
}