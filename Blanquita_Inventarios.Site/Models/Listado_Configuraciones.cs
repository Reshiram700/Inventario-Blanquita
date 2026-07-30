using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_Configuraciones
    {
        public int IdConfiguracion {  get; set; }
        public DateTime FechaRegistro {  get; set; }
        public string NombreInventario { get; set; }
        public string Servidor { get; set; }
        public string NombreInstancia { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string BaseDatos { get; set; }
        public string Almacen {  get; set; }     
        public bool Cerrado { get; set; }
        public bool Deshabilitado { get; set; }
    }
}