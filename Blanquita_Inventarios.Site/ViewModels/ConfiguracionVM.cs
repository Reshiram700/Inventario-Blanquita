using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class ConfiguracionVM
    {
        [Key]
        public int IdConfiguracion {  get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string NombreInventario { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Servidor { get; set; }
        public string NombreInstancia { get; set; }
        public string Puerto { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string BaseDatos { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Almacen {  get; set; }
    }
}