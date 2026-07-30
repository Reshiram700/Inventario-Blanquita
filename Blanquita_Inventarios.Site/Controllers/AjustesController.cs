using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Entities.Generics;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4);

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
        [ValidateAntiForgeryToken]
        public JsonResult Index(AjustesVM viewModel)
        {
            DBResponse<int> result = new DBResponse<int>();

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4);

            if (!isAdmin)
            {
                result.ExecutionOK = false;
                result.Message = "Debido a que no cuenta con permisos no es posible aplicar los Ajustes en el Inventario seleccionado.";
                return Json(result);
            }

            if (AjustesProgress.EnProceso)
            {
                result.ExecutionOK = false;
                result.Message = "Ya existe un proceso de ajustes ejecutándose.";
                return Json(result);
            }

            AjustesProgress.Reset();
            AjustesProgress.EnProceso = true;
            AjustesProgress.Inicio = DateTime.Now;
            AjustesProgress.Etapa = "Preparando proceso...";

            int idConfiguracion = viewModel.FiltroIdConfiguracion;

            ConexDIAPI conexion = new ConexDIAPI
            {
                Server = Config.DIAPI_Server,
                Company = Config.DIAPI_CompanyDB,
                UserName = Config.DIAPI_UserName,
                Password = Config.DIAPI_Password,
                DbUserName = Config.DIAPI_DbUserName,
                DbPassword = Config.DIAPI_DbPassword,
                UseTrusted = Config.DIAPI_UseTrusted
            };

            string directorioLog = Config.DirectorioLog;

            Task.Run(() =>
            {
                try
                {
                    new ConfiguracionesBL().Aply_Ajustes(
                        idConfiguracion,
                        conexion,
                        directorioLog);
                }
                catch (Exception ex)
                {
                    AjustesProgress.Finalizado = true;
                    AjustesProgress.Exitoso = false;
                    AjustesProgress.Mensaje = ex.Message;
                    AjustesProgress.Etapa = "Error";
                }
                finally
                {
                    AjustesProgress.EnProceso = false;
                }
            });

            result.ExecutionOK = true;
            result.Message = "Proceso iniciado correctamente.";

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetProgress()
        {
            TimeSpan tiempo = TimeSpan.Zero;

            if (AjustesProgress.Inicio != DateTime.MinValue)
                tiempo = DateTime.Now - AjustesProgress.Inicio;

            return Json(new
            {
                EnProceso = AjustesProgress.EnProceso,
                Etapa = AjustesProgress.Etapa,
                Total = AjustesProgress.Total,
                Procesados = AjustesProgress.Procesados,
                Porcentaje = AjustesProgress.Porcentaje,
                Tiempo = tiempo.ToString(@"hh\\:mm\\:ss"),

                Finalizado = AjustesProgress.Finalizado,
                Exitoso = AjustesProgress.Exitoso,
                Mensaje = AjustesProgress.Mensaje

            }, JsonRequestBehavior.AllowGet);
        }
    }
}