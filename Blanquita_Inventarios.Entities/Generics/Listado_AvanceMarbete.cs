using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_AvanceMarbete
    {
        public int IdMarbete { get; set; }
        public string Almacen { get; set; }
        public string Zona { get; set; }
        public int Marbete { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaUltimo { get; set; }
        public int TotalMinutos { get; set; }
        public string Tiempo { get; set; }
        public string Nombre { get; set; }
    }
}
