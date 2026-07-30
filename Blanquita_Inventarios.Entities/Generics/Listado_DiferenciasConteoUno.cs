using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_DiferenciasConteoUno
    {
        public string WhsCode { get; set; }
        public string ItmsGrpNam {  get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom {  get; set; }
        public int Onhand { get; set; }
        public int PorProcesar { get; set; }
        public int Contado { get; set; }
        public decimal DifPesoNeto { get; set; }
        public string Marbetes { get; set; }
    }
}
