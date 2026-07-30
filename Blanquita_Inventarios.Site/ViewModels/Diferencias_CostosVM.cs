using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Diferencias_CostosVM
    {
        public int FiltroIdConfiguracion { get; set; }
        public List<ControlDDL> FiltroInventarios { get; set; }
        public List<Listado_DiferenciasCostos> Listado {  get; set; }
    }
}