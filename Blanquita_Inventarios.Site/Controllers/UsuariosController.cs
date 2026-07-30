using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.Models;
using Blanquita_Inventarios.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class UsuariosController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 1 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            return View(Get_Usuarios(new Listado_UsuariosVM()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Listado_UsuariosVM viewModel)
        {
            return View(Get_Usuarios(viewModel));
        }

        public ActionResult RestablecerUsuarios()
        {
            return RedirectToAction("Index");
        }

        private Listado_UsuariosVM Get_Usuarios(Listado_UsuariosVM viewModel)
        {
            Listado_UsuariosVM listadoVM = new Listado_UsuariosVM();

            TempData["messages"] = new Dictionary<string, string[]>();

            List<Listado_Usuarios> listado = new List<Listado_Usuarios>();
            var listaPerfiles = new OtrosBL().Get_ListadoPerfiles("-Seleccione-").Data;

            listadoVM.FiltroNombre = viewModel.FiltroNombre;
            listadoVM.FiltroUsuario = viewModel.FiltroUsuario;
            listadoVM.FiltroEmail = viewModel.FiltroEmail;
            listadoVM.FiltroIdPerfil = viewModel.FiltroIdPerfil;
            listadoVM.FiltroPerfiles = listaPerfiles;

            DBResponse<List<Usuarios>> response = new UsuariosBL().Get_Usuarios(
                viewModel.FiltroNombre, 
                viewModel.FiltroUsuario,
                viewModel.FiltroEmail, 
                viewModel.FiltroIdPerfil);
            if (response.ExecutionOK)
            {

                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (Usuarios i in response.Data)
                    {
                        listado.Add(new Listado_Usuarios()
                        {
                            IdUsuario = i.IdUsuario,
                            Nombre = i.Nombre,
                            Usuario = i.Usuario,
                            Email = i.Email,
                            Perfil = i.Perfiles.Perfil,
                            Principal = i.Principal
                        });
                    }
                }
            }
            else
            {
                this.ShowNotificacion("error", "", response.Message, "4", "0");
            }

            listadoVM.Listado = listado;

            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return listadoVM;
        }

        public ActionResult Create()
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");
            
            UsuarioVM viewModel = new UsuarioVM();
            viewModel.IdUsuario = 0;

            var listadoPerfiles = new OtrosBL().Get_ListadoPerfiles("-Seleccione-").Data;
            viewModel.Perfiles = listadoPerfiles;
            TempData["Usuario_Perfiles"] = listadoPerfiles;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            
            List<ControlDDL> listadoPerfiles = (List<ControlDDL>)TempData["Usuario_Perfiles"];
            
            viewModel.Perfiles = listadoPerfiles;

            TempData["Usuario_Perfiles"] = listadoPerfiles;

            if (ModelState.IsValid)
            {
                DBResponse<Usuarios> response = new UsuariosBL().Upsert_Usuario(
                   new Usuarios()
                   {
                       IdUsuario = 0,
                       Nombre = viewModel.Nombre,
                       Usuario = viewModel.Usuario,
                       Password = viewModel.Password,
                       Email = viewModel.Email,  
                       IdPerfil = viewModel.IdPerfil,
                       Activo = true                       
                   });

                if (!response.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", response.Message, "4", "0");
                    return View(viewModel);
                }
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Verificar errores.", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Se ha guardado correctamente los datos del Usuario: " + viewModel.Nombre;
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return RedirectToAction("Index", "Usuarios");

            TempData["messages"] = new Dictionary<string, string[]>();            

            UsuarioVM viewModel = new UsuarioVM();
            viewModel.IdUsuario = 0;

            var listadoPerfiles = new OtrosBL().Get_ListadoPerfiles("-Seleccione-").Data;
            viewModel.Perfiles = listadoPerfiles;
            TempData["Usuario_Perfiles"] = listadoPerfiles;

            DBResponse<Usuarios> response = new UsuariosBL().Search_UsuarioByID(id.Value);
            if(response.ExecutionOK)
            {
                viewModel.IdUsuario = response.Data.IdUsuario;
                viewModel.Nombre = response.Data.Nombre;
                viewModel.Usuario = response.Data.Usuario;
                viewModel.Password = response.Data.Password;
                viewModel.Email = response.Data.Email;
                viewModel.IdPerfil = response.Data.IdPerfil;
            }
            else
            {
                TempData["MensajeAIndex"] = response.Message;
                return RedirectToAction("Index", "Usuarios");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(UsuarioVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            List<ControlDDL> listadoPerfiles = (List<ControlDDL>)TempData["Usuario_Perfiles"];

            viewModel.Perfiles = listadoPerfiles;

            TempData["Usuario_Perfiles"] = listadoPerfiles;

            if (ModelState.IsValid)
            {
                DBResponse<Usuarios> response = new UsuariosBL().Upsert_Usuario(
                   new Usuarios()
                   {
                       IdUsuario = viewModel.IdUsuario,
                       Nombre = viewModel.Nombre,
                       Usuario = viewModel.Usuario,
                       Password = viewModel.Password,
                       Email = viewModel.Email,
                       IdPerfil = viewModel.IdPerfil
                   });

                if (!response.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", response.Message, "4", "0");
                    return View(viewModel);
                }
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Verificar errores.", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Se ha guardado correctamente los datos del Usuario: " + viewModel.Nombre;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<Usuarios> response = new UsuariosBL().Delete_Usuario(id.Value);
            if (response.ExecutionOK)
            {
                TempData["MensajeAIndex"] = "El Usuario se ha eliminado correctamente.";
            }
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }
    }
}