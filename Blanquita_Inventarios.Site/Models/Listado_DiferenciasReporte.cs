using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_DiferenciasReporte
    {
        public string Marbetes {  get; set; }
        public string CodigoArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public string Uom1 { get; set; }
        public decimal Onhand {  get; set; }
        public decimal Contado { get; set; }
        public decimal Precio { get; set; }
        public decimal VNetaPza { get; set; }
        public decimal VNetaMonto { get; set; }
    }
}