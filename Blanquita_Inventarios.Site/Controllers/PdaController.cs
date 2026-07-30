using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.Models;
using Blanquita_Inventarios.Site.ViewModels;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class PdaController : Controller
    {
        // GET: Pda
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            ArticulosPDAVM listadoVM = new ArticulosPDAVM();
            List<Listado_ArticulosPDA> listado = new List<Listado_ArticulosPDA>();

            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;
            TempData["ArticulosPDA_Inventarios"] = listaInventarios;
            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;

            listadoVM.Listado = listado;

            return View(listadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ArticulosPDAVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            ArticulosPDAVM listadoVM = new ArticulosPDAVM();
            List<Listado_ArticulosPDA> listado = new List<Listado_ArticulosPDA>();
            
            var listaInventarios = (List<ControlDDL>)TempData["ArticulosPDA_Inventarios"];
            listadoVM.FiltroInventarios = listaInventarios;
            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;

            try
            {
                DBResponse<List<Listado_ArticulosPDA>> response = new ConfiguracionesBL().Get_PDAArticulos(listadoVM.FiltroIdConfiguracion);
                if (response.ExecutionOK)
                {
                    listado = response.Data;
                    TempData["ArticulosPDA_Listado"] = listado;
                }
                else
                {
                    this.ShowNotificacion("error", "", response.Message, "4", "0");
                }

            }
            catch (Exception ex)
            {
                this.ShowNotificacion("error", "", ex.Message, "4", "0");
            }

            listadoVM.Listado = listado;

            TempData["ArticulosPDA_Inventarios"] = listaInventarios;

            return View(listadoVM);
        }

        [HttpPost]
        public ActionResult ExportarExcel()
        {
            var response = new DBResponse<string>();

            try
            {
                List<Listado_ArticulosPDA> listado = (List<Listado_ArticulosPDA>)TempData["ArticulosPDA_Listado"];
                TempData["ArticulosPDA_Listado"] = listado;

                if (listado != null && listado.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PDA");
                    dt.Columns.Add("Cantidad_Articulos");

                    foreach (var item in listado)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.PDA;
                        row[1] = item.Cantidad;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddhhmmss") + "_CantidadArticulosPDA.xls";
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Cantidad Articulos PDA", nombreArchivo);
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }
    }
}