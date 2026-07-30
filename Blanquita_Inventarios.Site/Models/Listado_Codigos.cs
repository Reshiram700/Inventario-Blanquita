using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_Codigos
    {
        public string Codigo { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string UsuarioGenero { get; set; }
        public string UsuarioUtilizo { get; set; }
        public string Accion {  get; set; }
        public string Estatus {  get; set; }
    }
}