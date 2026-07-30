using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Session["UserAdmin"] = null;

            LoginVM viewModel = new LoginVM();

            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione Inventario -").Data;
            viewModel.FiltroInventarios = listaInventarios;
            TempData["Login_Inventarios"] = listaInventarios;

            viewModel.FiltroInventarios = listaInventarios;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            var listaInventarios = (List<ControlDDL>)TempData["Login_Inventarios"];
            viewModel.FiltroInventarios = listaInventarios;

            TempData["Login_Inventarios"] = listaInventarios;

            if (ModelState.IsValid)
            {
                if(viewModel.FiltroIdInventario == 0)
                {
                    this.ShowNotificacion("error", "Error", "Seleccione un Inventario.", "0", "0");
                    return View(viewModel);
                }

                DBResponse<UsuarioSesion> obj = new UsuariosBL().User_Login(viewModel.Usuario, viewModel.Password, viewModel.FiltroIdInventario);
                if (obj.ExecutionOK && obj.Data != null)
                {
                    Session["UserAdmin"] = obj.Data;
                    return RedirectToAction("Home", "Home");
                }

                this.ShowNotificacion("error", "Error", "Usuario o contraseña incorrectos.", "0", "0");
            }

            return View(viewModel);
        }

        public ActionResult OlvidastePassword()
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OlvidastePassword(InicioVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            if (ModelState.IsValid)
            {
                if(String.IsNullOrEmpty(viewModel.Usuario))
                {
                    this.ShowNotificacion("error", "Error", "Ingrese su usuario", "0", "0");
                }
                else
                {
                    DBResponse<Usuarios> response = new UsuariosBL().Search_UsuarioByUser(viewModel.Usuario);
                    if (!response.ExecutionOK)
                    {
                        this.ShowNotificacion("error", "Error", response.Message, "0", "0");
                    }
                    else
                    {
                        ListDictionary dictionary = new ListDictionary();
                        dictionary.Add("{Nombre}", response.Data.Nombre);
                        dictionary.Add("{Password}", response.Data.Password);
                        dictionary.Add("{urlSitio}", Config.UrlSitio);
                        string enviar = Correo.SendEmail_General("OlvidoPassword.html", "Sistema de Toma de Inventarios - Recuperación de Password", response.Data.Email, dictionary);
                        if (!String.IsNullOrEmpty(enviar))
                        {
                            this.ShowNotificacion("error", "Error", enviar, "0", "0");
                        }
                        else
                        {
                            viewModel.Usuario = "";
                            this.ShowNotificacion("success", "", "En breve recibira un email al correo registrado en su usuario.", "0", "0");
                        }
                    }
                }
            }

            return View(viewModel);
        }

        public ActionResult ActualizarPassword()
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarPassword(InicioVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (ModelState.IsValid) {

                string msjError = "";

                if (String.IsNullOrEmpty(viewModel.Usuario))
                    msjError = "Ingrese su usuario.";

                if (String.IsNullOrEmpty(viewModel.Password))
                    msjError += "Ingrese su contraseña actual.";

                if (String.IsNullOrEmpty(viewModel.PasswordNew01) || String.IsNullOrEmpty(viewModel.PasswordNew02))
                    msjError += "Verifique que haya ingresado su contraseña nueva y la haya confirmado.";

                if (viewModel.PasswordNew01 != viewModel.PasswordNew02)
                    msjError += "Revise que la contraseña nueva sea la misma que su confirmación.";

                if(String.IsNullOrEmpty(msjError))
                {
                    DBResponse<Usuarios> response = new UsuariosBL().Update_Password(viewModel.Usuario, viewModel.Password, viewModel.PasswordNew01);
                    if (response.ExecutionOK)
                    {
                        viewModel = new InicioVM();
                        this.ShowNotificacion("success", "", "Su contraseña ha sido actualizada.", "0", "0");
                    }
                    else
                    {
                        this.ShowNotificacion("error", "Error", response.Message, "0", "0");
                    }
                }
                else{
                    this.ShowNotificacion("error", "Error", msjError, "0", "0");
                }
            }

            return View(viewModel);
        }

        public ActionResult Home()
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}