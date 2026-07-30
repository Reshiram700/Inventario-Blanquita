using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_AvanceDetalleMarbete
    {
        public int IdConfiguracion { get; set; }
        public int Folio { get; set; }
        public string Almacen { get; set; }
        public int Marbete { get; set; }
        public string CodigoBarras { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Cantidad { get; set; }
        public string UOM { get; set; }
        public DateTime TimeCount1 { get; set; }
        public string Nombre { get; set; }
    }
}
