using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Diferencias_ConteoUnoVM
    {
        public List<ControlDDL> FiltroInventarios { get; set; }
        public int FiltroIdConfiguracion { get; set; }
        public string FiltroBusca {  get; set; }
        public string FiltroValorItmsGrpNam {  get; set; }
        public List<ControlDDL2> FiltroItmsGrpNam { get; set; }
        public string FiltroValorItmsGrpNam2 { get; set; }
        public List<ControlDDL2> FiltroItmsGrpNam2 { get; set; }
        public int FiltroIdZonaReporte { get; set; }
        public List<ControlDDL> FiltroZonasReporte { get; set; }
        public List<Listado_DiferenciasConteoUno> Listado { get; set; }
    }
}