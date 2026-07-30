using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class DatosBarcode
    {
        public int Tipo { get; set; }
        public int IdMarbete { get; set; }
        public int Marbete { get; set; }
        public string Codigo { get; set; }
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public string Uom { get; set; }
        public decimal BaseQty { get; set; }
        public decimal Costo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Cantidad2 { get; set; }
        public decimal Precio { get; set; }
        public string NombrePDA { get; set; }
    }
}
