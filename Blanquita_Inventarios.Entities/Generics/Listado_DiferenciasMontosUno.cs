using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_DiferenciasMontosUno
    {
        public string WhsCode { get; set; }
        public string ItmsGrpName { get; set; }
        public decimal TotalSAP { get; set; }
        public decimal TotalContado { get; set; }
        public decimal TotalDesviacion { get; set; }
        public decimal PorcentajeDif { get; set; }
    }
}
