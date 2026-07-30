using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class UsuarioVM
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        public string Email { get; set; }

        [Range(1, 9999999, ErrorMessage = "Seleccione un Perfil de Usuario")]
        public int IdPerfil { get; set; }
        public List<ControlDDL> Perfiles { get; set; }
    }
}