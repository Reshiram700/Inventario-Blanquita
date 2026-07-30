using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_AvanceZona
    {
        public int IdZona { get; set; }
        public string Almacen { get; set; }
        public string Zona { get; set; }
        public decimal Avance { get; set; }
        public int Minutos { get; set; }
        public string Tiempo { get; set; }
        public int Cerrados { get; set; }
        public int TotalMarbetes { get; set; }
        public int MarbetesPorCapturar { get; set; }
        public decimal Monto { get; set; }
        public string Estatus { get; set; }
    }
}
