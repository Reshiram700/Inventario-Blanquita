using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.DataAccess
{
    public class UsuariosDA
    {
        public DBResponse<UsuarioSesion> User_Login(string usuario, string password, int idConfiguracion)
        {
            DBResponse<UsuarioSesion> response = new DBResponse<UsuarioSesion>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var entry = (from a in db.Usuarios.Include("Perfiles")
                                 where a.Usuario == usuario
                                    && a.Password == password
                                    && a.Activo == true
                                 select a).FirstOrDefault();

                    if (entry != null)
                    {
                        UsuarioSesion user = new UsuarioSesion
                        {
                            Id = entry.IdUsuario,
                            Usuario = usuario,
                            Nombre = entry.Nombre,
                            IdPerfil = entry.IdPerfil,
                            Perfil = entry.Perfiles.Perfil,
                            IdConfiguracion = idConfiguracion
                        };

                        response.ExecutionOK = true;
                        response.Data = user;
                    }
                    else
                    {
                        response.Message = "El usuario que ingreso no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<UsuarioSesion> LoginApp(UserLogin login)
        {
            DBResponse<UsuarioSesion> response = new DBResponse<UsuarioSesion>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var entry = (from a in db.Usuarios.Include("Perfiles")
                                 where a.Usuario == login.Usuario
                                    && a.Password == login.Password
                                    && a.Activo == true
                                 select a).FirstOrDefault();

                    if (entry != null)
                    {
                        if (entry.IdPerfil == 2 || entry.IdPerfil == 4)
                        {

                            UsuarioSesion user = new UsuarioSesion
                            {
                                Id = entry.IdUsuario,
                                Usuario = entry.Usuario,
                                Nombre = entry.Nombre,
                                IdPerfil = entry.IdPerfil,
                                Perfil = entry.Perfiles.Perfil
                            };

                            response.ExecutionOK = true;
                            response.Data = user;
                        }
                        else
                        {
                            response.Message = "El usuario no tiene permiso para ingresar a esta aplicación";
                        }
                    }
                    else
                    {
                        response.Message = "El usuario que ingreso no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Usuarios> Search_UsuarioByUser(string usuario)
        {
            DBResponse<Usuarios> response = new DBResponse<Usuarios>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var entry = (from a in db.Usuarios.Include("Perfiles")
                                 where a.Usuario == usuario
                                    && a.Activo == true
                                 select a).FirstOrDefault();

                    if (entry != null)
                    {
                        response.ExecutionOK = true;
                        response.Data = entry;
                    }
                    else
                    {
                        response.Message = "El usuario que ingreso no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Usuarios> Update_Password(string usuario, string password, string passwordNew)
        {
            DBResponse<Usuarios> response = new DBResponse<Usuarios>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var entry = (from a in db.Usuarios.Include("Perfiles")
                                 where a.Usuario == usuario
                                    && a.Password == password
                                    && a.Activo == true
                                 select a).FirstOrDefault();

                    if (entry != null)
                    {
                        var dbItem = db.Usuarios.Find(entry.IdUsuario);
                        dbItem.Password = passwordNew;
                        db.SaveChanges();

                        response.ExecutionOK = true;
                        response.Data = dbItem;
                    }
                    else
                    {
                        response.Message = "El usuario que ingreso no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<List<Usuarios>> Get_Usuarios(string nombre, string usuario, string email, int idPerfil)
        {
            DBResponse<List<Usuarios>> response = new DBResponse<List<Usuarios>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from a in db.Usuarios.Include("Perfiles")
                                 where a.Activo == true
                                    && (String.IsNullOrEmpty(nombre) || (!String.IsNullOrEmpty(nombre) && a.Nombre.Contains(nombre)))
                                    && (String.IsNullOrEmpty(usuario) || (!String.IsNullOrEmpty(usuario) && a.Usuario.Contains(usuario)))
                                    && (String.IsNullOrEmpty(email) || (!String.IsNullOrEmpty(email) && a.Email.Contains(email)))
                                    && (idPerfil == 0 || (idPerfil > 0 && a.IdPerfil == idPerfil))
                                 select a).ToList();

                    response.ExecutionOK = true;
                    response.Data = query;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Usuarios> Search_UsuarioByID(int idUsuario)
        {
            DBResponse<Usuarios> response = new DBResponse<Usuarios>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = db.Usuarios.Find(idUsuario);

                    if (query != null)
                    {
                        response.ExecutionOK = true;
                        response.Data = query;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Usuario solicitado.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Usuarios> Search_ExisteUsuario(int idUsuario, string usuario)
        {
            DBResponse<Usuarios> response = new DBResponse<Usuarios>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var entry = (from a in db.Usuarios.Include("Perfiles")
                                 where a.Usuario == usuario
                                    && a.IdUsuario != idUsuario
                                 select a).FirstOrDefault();

                    response.ExecutionOK = true;
                    response.Data = entry;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Usuarios> Upsert_Usuario(Usuarios usuario)
        {
            DBResponse<Usuarios> response = new DBResponse<Usuarios>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    if (usuario.IdUsuario == 0)
                    {
                        usuario.FechaRegistro = DateTime.Now;
                        db.Usuarios.Add(usuario);
                        db.SaveChanges();

                        response.ExecutionOK = true;
                        response.Data = usuario;
                    }
                    else
                    {
                        var entry = db.Usuarios.Find(usuario.IdUsuario);
                        if (entry != null)
                        {
                            entry.Nombre = usuario.Nombre;
                            entry.Usuario = usuario.Usuario;
                            entry.Password = usuario.Password;
                            entry.Email = usuario.Email;
                            entry.IdPerfil = usuario.IdPerfil;
                            db.SaveChanges();

                            response.ExecutionOK = true;
                            response.Data = usuario;
                        }
                        else
                        {
                            response.Message = "El usuario no existe";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Usuarios> Delete_Usuario(int idUsuario)
        {
            DBResponse<Usuarios> response = new DBResponse<Usuarios>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {

                    var entry = db.Usuarios.Find(idUsuario);
                    if (entry != null)
                    {
                        entry.Activo = false;
                        entry.FechaEliminado = DateTime.Now;
                        db.SaveChanges();

                        response.ExecutionOK = true;
                        response.Data = entry;
                    }
                    else
                    {
                        response.Message = "El usuario no existe";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }
    }
}
