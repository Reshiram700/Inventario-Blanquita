using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Diferencias_MontosUnoVM
    {
        public int FiltroIdConfiguracion { get; set; }
        public List<ControlDDL> FiltroInventarios { get; set; }

        public List<Listado_DiferenciasMontosUno> Listado {  get; set; }

        public decimal TotalSAP {  get; set; }
        public decimal TotalContado { get; set; }
        public decimal TotalDesviacion { get; set; }
        public decimal PorcentajeDif {  get; set; }
    }
}