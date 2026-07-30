using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class ProductosAjustar
    {
        public string Itemcode { get; set; }
        public decimal Contado { get; set; }
        public decimal Onhand { get; set; }
        public decimal Diferencia {  get; set; }
        public decimal Precio { get; set; }
        public string Tipo { get; set; }
        public int UomEntry {  get; set; }
    }
}
