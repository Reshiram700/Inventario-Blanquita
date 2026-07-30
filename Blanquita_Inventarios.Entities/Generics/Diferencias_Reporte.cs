using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Diferencias_Reporte
    {
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Marbetes { get; set; }
        public string CodigoArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public string Categoria { get; set; }
        public string Uom1 { get; set; }
        public decimal Onhand { get; set; }
        public decimal Contado { get; set; }
        public decimal Precio { get; set; }
        public decimal VNetaPza { get; set; }
        public decimal VNetaMonto { get; set; }
    }
}
