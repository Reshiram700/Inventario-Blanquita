using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class Listado_UsuariosVM
    {
        public string FiltroNombre { get; set; }
        public string FiltroUsuario { get; set; }
        public string FiltroEmail { get; set; }
        public int FiltroIdPerfil { get; set; }
        public List<ControlDDL> FiltroPerfiles { get; set; }
        public List<Listado_Usuarios> Listado { get; set; }
    }
}