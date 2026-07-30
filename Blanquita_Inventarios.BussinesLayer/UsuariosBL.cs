using Blanquita_Inventarios.DataAccess;
using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.BussinesLayer
{
    public class UsuariosBL
    {
        UsuariosDA conex = new UsuariosDA();

        public DBResponse<UsuarioSesion> User_Login(string usuario, string password, int idConfiguracion)
        {
            return conex.User_Login(usuario, password, idConfiguracion);
        }

        public DBResponse<UsuarioSesion> LoginApp(UserLogin login)
        {
            return conex.LoginApp(login);
        }

        public DBResponse<Usuarios> Search_UsuarioByUser(string usuario)
        {
            return conex.Search_UsuarioByUser(usuario);
        }

        public DBResponse<Usuarios> Update_Password(string usuario, string password, string passwordNew)
        {
            return conex.Update_Password(usuario, password, passwordNew);
        }

        public DBResponse<List<Usuarios>> Get_Usuarios(string nombre, string usuario, string email, int idPerfil)
        {
            return conex.Get_Usuarios(nombre, usuario, email, idPerfil);
        }

        public DBResponse<Usuarios> Search_UsuarioByID(int idUsuario)
        {
            return conex.Search_UsuarioByID(idUsuario);
        }

        public DBResponse<Usuarios> Search_ExisteUsuario(int idUsuario, string usuario)
        {
            return conex.Search_ExisteUsuario(idUsuario, usuario);
        }

        public DBResponse<Usuarios> Upsert_Usuario(Usuarios usuario)
        {
            string mensajeError = String.Empty;
            if(ValidarUsuario(usuario, out mensajeError))
            {
                return conex.Upsert_Usuario(usuario);
            }
            else
            {
                return new DBResponse<Usuarios> { ExecutionOK = false, Message = mensajeError };
            }
            
        }

        public bool ValidarUsuario(Usuarios usuario, out string mensajeError)
        {
            bool result = false;
            mensajeError = String.Empty;

            DBResponse<Usuarios> existe = conex.Search_ExisteUsuario(usuario.IdUsuario, usuario.Usuario);
            if(existe.ExecutionOK)
            {
                if (existe.Data != null)
                    mensajeError = "El Usuario fue usado por otro registro, intenta con otro.";
            }
            else            
                mensajeError = existe.Message;            

            if(String.IsNullOrEmpty(mensajeError))
                result = true;

            return result;
        }

        public DBResponse<Usuarios> Delete_Usuario(int idUsuario)
        {
            return conex.Delete_Usuario(idUsuario);
        }
    }
}
