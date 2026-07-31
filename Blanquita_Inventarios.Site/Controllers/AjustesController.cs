using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Entities.Generics;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class AjustesController : Controller
    {
        // GET: Ajustes
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            AjustesVM listadoVM = new AjustesVM();

            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;

            TempData["Ajustes_Inventarios"] = listaInventarios;

            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return View(listadoVM);
        }

        [HttpPost]
        public JsonResult Index(AjustesVM viewModel)
        {
            DBResponse<int> result = new DBResponse<int>();

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
            {
                result.Message = "Debido a que no cuenta con permisos no es posible aplicar los Ajustes en el Inventario seleccionado.";
                return Json(result);
            }

            try
            {
                result = new ConfiguracionesBL().Aply_Ajustes(viewModel.FiltroIdConfiguracion,
                    new ConexDIAPI
                    {
                        Server = Config.DIAPI_Server,
                        Company = Config.DIAPI_CompanyDB,
                        UserName = Config.DIAPI_UserName,
                        Password = Config.DIAPI_Password,
                        DbUserName = Config.DIAPI_DbUserName,
                        DbPassword = Config.DIAPI_DbPassword,
                        UseTrusted = Config.DIAPI_UseTrusted
                    }, Config.DirectorioLog);               

                if (result.ExecutionOK)
                {
                    TempData["MensajeAIndex"] = "Se aplicaron los ajustes correctamente";
                }
                else
                {
                    result.Message = "No fue posible realizar todos los ajustes debido a.... " + result.Message + ", Revise e intente nuevamente aplicar los ajustes";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (ex.InnerException != null)
                    result.Message += " (" + ex.InnerException.Message + ")";
            }
                        
            return Json(result);
        }
    }
}