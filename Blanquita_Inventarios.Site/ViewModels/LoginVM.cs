using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.ViewModels
{
    public class LoginVM
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int FiltroIdInventario { get; set; }
        public List<ControlDDL> FiltroInventarios { get; set; }
    }
}