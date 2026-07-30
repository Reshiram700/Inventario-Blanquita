namespace Blanquita_Inventarios.Entities
{
    public class UsuarioSesion
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public int IdPerfil { get; set; }
        public string Perfil { get; set; }
        public int IdConfiguracion { get; set; }        
    }
}
