using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class MarbeteDescargado
    {
        public int IdConfiguracion { get; set; }
        public int Marbete {  get; set; }
        public DateTime FechaInicio { get; set; }
        public string NombrePDA { get; set; }
    }
}
