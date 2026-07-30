namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_Usuarios
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public bool Principal { get; set; }
    }
}