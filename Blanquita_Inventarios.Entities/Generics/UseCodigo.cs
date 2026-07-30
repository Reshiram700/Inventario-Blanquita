using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class UseCodigo
    {
        public int IdConfiguracion {  get; set; }
        public string Codigo { get; set;}
        public string Nombre { get; set;}
        public int IdAccion { get; set;}
        public int IdUsuario { get; set;}
    }
}
