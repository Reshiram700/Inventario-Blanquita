using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Resultado_Capturas
    {
        public int Folio {  get; set; }
        public int Marbete { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string Conteo { get; set; }
        public string Nombre { get; set; }
    }
}
