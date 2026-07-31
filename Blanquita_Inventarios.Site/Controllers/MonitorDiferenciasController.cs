using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.Models;
using Blanquita_Inventarios.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class MonitorDiferenciasController : Controller
    {
        public ActionResult Capturas()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_CapturasVM listadoVM = new Diferencias_CapturasVM();
            Tabla_ResultadoCapturas resultadoCapturas = new Tabla_ResultadoCapturas();
            List<Listado_DiferenciasCapturas> listado = new List<Listado_DiferenciasCapturas>();

            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;
            listadoVM.FiltroIdConteo = 1;
            var listaConteos = new OtrosBL().Get_ListadoConteos().Data;
            listadoVM.FiltroConteos = listaConteos;

            resultadoCapturas.Listado = listado;
            listadoVM.ResultadoCapturas = resultadoCapturas;

            TempData["DiferenciasCapturas_Inventarios"] = listaInventarios;
            TempData["DiferenciasCapturas_Conteos"] = listaConteos;

            return View(listadoVM);
        }

        [HttpPost]
        public ActionResult Procesa_Capturas()
        {
            Tabla_ResultadoCapturas resultadoCapturas = new Tabla_ResultadoCapturas();
            List<Listado_DiferenciasCapturas> listado = new List<Listado_DiferenciasCapturas>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                int marbete = int.Parse(form["marbete"].ToString());
                int idConteo = int.Parse(form["idConteo"].ToString());
                string codigoArticulo = form["codigoArticulo"].ToString();

                DBResponse<List<Resultado_Capturas>> response = new ConfiguracionesBL().Get_Procesado(
                    idConfiguracion,
                    marbete,
                    idConteo,
                    codigoArticulo);
                if (response.ExecutionOK)
                {
                    foreach (var i in response.Data)
                    {
                        listado.Add(new Listado_DiferenciasCapturas
                        {
                            Folio = i.Folio,
                            Marbete = i.Marbete,
                            CodigoArticulo = i.CodigoProducto,
                            DescripcionArticulo = i.Descripcion,
                            Conteo = i.Conteo,
                            NombreMarbete = i.Nombre
                        });
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(500, response.Message);
                }
            }
            catch (Exception ex)
            {
                string msjError = ex.Message;
                if (ex.InnerException != null)
                    msjError += ex.InnerException.Message;

                return new HttpStatusCodeResult(500, msjError);
            }

            resultadoCapturas.Listado = listado;
            TempData["DiferenciasCapturas_Capturas"] = listado;

            return PartialView("Vista_ListadoCapturas", resultadoCapturas);
        }

        [HttpPost]
        public ActionResult ExcelCapturas()
        {
            var response = new DBResponse<string>();

            try
            {
                List<Listado_DiferenciasCapturas> listado = (List<Listado_DiferenciasCapturas>)TempData["DiferenciasCapturas_Capturas"];
                TempData["DiferenciasCapturas_Capturas"] = listado;

                if (listado != null && listado.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Folio");
                    dt.Columns.Add("Marbete");
                    dt.Columns.Add("CodigoArticulo");
                    dt.Columns.Add("DescripcionArticulo");
                    dt.Columns.Add("ConteoActivo");
                    dt.Columns.Add("NombreMarbete");

                    foreach (var item in listado)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.Folio;
                        row[1] = item.Marbete;
                        row[2] = item.CodigoArticulo;
                        row[3] = item.DescripcionArticulo;
                        row[4] = item.Conteo;
                        row[5] = item.NombreMarbete;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddhhmmss") + "_DiferenciasCapturas.xls";
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Diferencias de Capturas", nombreArchivo);
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public ActionResult Costos()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_CostosVM listadoVM = new Diferencias_CostosVM();
            List<Listado_DiferenciasCostos> listado = new List<Listado_DiferenciasCostos>();

            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;

            listadoVM.Listado = listado;
            TempData["Diferencias_CostoCeroInventarios"] = listaInventarios;

            return View(listadoVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Costos(Diferencias_CostosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_CostosVM listadoVM = new Diferencias_CostosVM();
            List<Listado_DiferenciasCostos> listado = new List<Listado_DiferenciasCostos>();

            var listadoInventarios = (List<ControlDDL>)TempData["Diferencias_CostoCeroInventarios"];
            listadoVM.FiltroInventarios = listadoInventarios;
            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;

            try
            {
                DBResponse<List<Report_CostoCeroByInventario_Result>> response = new ConfiguracionesBL().Get_CostoCeroInactivos(listadoVM.FiltroIdConfiguracion);
                if (response.ExecutionOK)
                {
                    foreach (var resultado in response.Data)
                    {
                        listado.Add(new Listado_DiferenciasCostos
                        {
                            ItemCode = resultado.ItemCode,
                            Descripcion = resultado.Descripcion,
                            Estatus = resultado.Estatus
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowNotificacion("error", "", ex.Message, "4", "0");
            }

            listadoVM.Listado = listado;
            TempData["Diferencias_CostoCeroInventarios"] = listadoInventarios;

            // ===== GUARDAR EN TEMPDATA PARA EL EXCEL =====
            TempData["DiferenciasCostos_Listado"] = listado;
            // ============================================

            return View(listadoVM);
        }

        public ActionResult MontosUno()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_MontosUnoVM listadoVM = new Diferencias_MontosUnoVM();
            List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;

            TempData["DiferenciasMontosUno_Inventarios"] = listaInventarios;

            listadoVM.Listado = listado;
            listadoVM.TotalSAP = 0;
            listadoVM.TotalContado = 0;
            listadoVM.TotalDesviacion = 0;
            listadoVM.PorcentajeDif = 0;

            return View(listadoVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MontosUno(Diferencias_MontosUnoVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_MontosUnoVM listadoVM = new Diferencias_MontosUnoVM();
            List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

            var listadoInventarios = (List<ControlDDL>)TempData["DiferenciasMontosUno_Inventarios"];
            listadoVM.FiltroInventarios = listadoInventarios;
            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;

            try
            {
                DBResponse<List<Listado_DiferenciasMontosUno>> response = new ConfiguracionesBL().Get_MontosUno(listadoVM.FiltroIdConfiguracion);
                if (response.ExecutionOK)
                {
                    listado = response.Data;
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
            listadoVM.TotalSAP = listado.Sum(s => s.TotalSAP);
            listadoVM.TotalContado = listado.Sum(s => s.TotalContado);
            listadoVM.TotalDesviacion = listado.Sum(s => s.TotalDesviacion);
            listadoVM.PorcentajeDif = listadoVM.TotalSAP != 0 ? ((listadoVM.TotalDesviacion / listadoVM.TotalSAP) * 100) : 0;

            TempData["DiferenciasMontosUno_Inventarios"] = listadoInventarios;
            TempData["DiferenciasMontosUno_Listado"] = listado;

            return View(listadoVM);
        }

        public ActionResult ExcelMontosUno()
        {
            var response = new DBResponse<string>();

            try
            {
                List<Listado_DiferenciasMontosUno> listado = (List<Listado_DiferenciasMontosUno>)TempData["DiferenciasMontosUno_Listado"];
                TempData["DiferenciasMontosUno_Listado"] = listado;

                if (listado != null && listado.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("WhsCode");
                    dt.Columns.Add("ItmsGrpName");
                    dt.Columns.Add("TotalSAP");
                    dt.Columns.Add("TotalContado");
                    dt.Columns.Add("TotalDesviacion");
                    dt.Columns.Add("PorcDif");

                    foreach (var item in listado)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.WhsCode;
                        row[1] = item.ItmsGrpName;
                        row[2] = item.TotalSAP;
                        row[3] = item.TotalContado;
                        row[4] = item.TotalDesviacion;
                        row[5] = item.PorcentajeDif;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddhhmmss") + "_DiferenciasMontos1.xls";
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Diferencias de Montos Conteo 1", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public ActionResult MontosDos()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_MontosUnoVM listadoVM = new Diferencias_MontosUnoVM();
            List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;

            TempData["DiferenciasMontosDos_Inventarios"] = listaInventarios;

            listadoVM.Listado = listado;
            listadoVM.TotalSAP = 0;
            listadoVM.TotalContado = 0;
            listadoVM.TotalDesviacion = 0;
            listadoVM.PorcentajeDif = 0;

            return View(listadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MontosDos(Diferencias_MontosUnoVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_MontosUnoVM listadoVM = new Diferencias_MontosUnoVM();
            List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

            var listadoInventarios = (List<ControlDDL>)TempData["DiferenciasMontosDos_Inventarios"];
            listadoVM.FiltroInventarios = listadoInventarios;
            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;

            try
            {
                DBResponse<List<Listado_DiferenciasMontosUno>> response = new ConfiguracionesBL().Get_MontosDos(listadoVM.FiltroIdConfiguracion);
                if (response.ExecutionOK)
                {
                    listado = response.Data;
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
            listadoVM.TotalSAP = listado.Sum(s => s.TotalSAP);
            listadoVM.TotalContado = listado.Sum(s => s.TotalContado);
            listadoVM.TotalDesviacion = listado.Sum(s => s.TotalDesviacion);
            listadoVM.PorcentajeDif = listadoVM.TotalSAP != 0 ? ((listadoVM.TotalDesviacion / listadoVM.TotalSAP) * 100) : 0;

            TempData["DiferenciasMontosDos_Listado"] = listado;
            TempData["DiferenciasMontosDos_Inventarios"] = listadoInventarios;

            return View(listadoVM);
        }

        public ActionResult ExcelMontosDos()
        {
            var response = new DBResponse<string>();

            try
            {
                List<Listado_DiferenciasMontosUno> listado = (List<Listado_DiferenciasMontosUno>)TempData["DiferenciasMontosDos_Listado"];
                TempData["DiferenciasMontosDos_Listado"] = listado;

                if (listado != null && listado.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("WhsCode");
                    dt.Columns.Add("ItmsGrpName");
                    dt.Columns.Add("TotalSAP");
                    dt.Columns.Add("TotalContado");
                    dt.Columns.Add("TotalDesviacion");
                    dt.Columns.Add("PorcDif");

                    foreach (var item in listado)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.WhsCode;
                        row[1] = item.ItmsGrpName;
                        row[2] = item.TotalSAP;
                        row[3] = item.TotalContado;
                        row[4] = item.TotalDesviacion;
                        row[5] = item.PorcentajeDif;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddhhmmss") + "_DiferenciasMontos2.xls";
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Diferencias de Montos Conteo 2", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public ActionResult ConteoUno()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_ConteoUnoVM listadoVM = new Diferencias_ConteoUnoVM();
            List<Listado_DiferenciasConteoUno> listado = new List<Listado_DiferenciasConteoUno>();

            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;

            listadoVM.FiltroValorItmsGrpNam = "";
            listadoVM.FiltroValorItmsGrpNam2 = "";
            var listaCategorias = new OtrosBL().Get_ListadoCategorias("- Seleccione -").Data;

            listadoVM.FiltroInventarios = listaInventarios;
            listadoVM.FiltroItmsGrpNam = listaCategorias;
            listadoVM.FiltroItmsGrpNam2 = listaCategorias;
            listadoVM.FiltroIdZonaReporte = 0;
            listadoVM.FiltroZonasReporte = new List<ControlDDL>();

            TempData["DiferenciasConteoUno_Inventarios"] = listaInventarios;
            TempData["DiferenciasConteoUno_Categorias"] = listaCategorias;

            return View(listadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConteoUno(Diferencias_ConteoUnoVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_ConteoUnoVM listadoVM = new Diferencias_ConteoUnoVM();
            List<Listado_DiferenciasConteoUno> listado = new List<Listado_DiferenciasConteoUno>();

            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;
            var listaInventarios = (List<ControlDDL>)TempData["DiferenciasConteoUno_Inventarios"];
            var listaCategorias = (List<ControlDDL2>)TempData["DiferenciasConteoUno_Categorias"];

            listadoVM.FiltroInventarios = listaInventarios;
            listadoVM.FiltroBusca = viewModel.FiltroBusca;
            listadoVM.FiltroValorItmsGrpNam = viewModel.FiltroValorItmsGrpNam;
            listadoVM.FiltroValorItmsGrpNam2 = "";
            listadoVM.FiltroItmsGrpNam = listaCategorias;
            listadoVM.FiltroItmsGrpNam2 = listaCategorias;
            listadoVM.FiltroZonasReporte = new List<ControlDDL>();

            try
            {
                if (listadoVM.FiltroIdConfiguracion > 0)
                {
                    var listaZonas = new OtrosBL().Get_ListadoZonasByConfiguracion(listadoVM.FiltroIdConfiguracion, "- Seleccione -").Data;
                    listadoVM.FiltroZonasReporte = listaZonas;
                    TempData["DiferenciasConteoUno_ZonasConfiguracion"] = listaZonas;

                    DBResponse<List<Listado_DiferenciasConteoUno>> response = new ConfiguracionesBL().Get_ConteoUno(listadoVM.FiltroIdConfiguracion, listadoVM.FiltroBusca, listadoVM.FiltroValorItmsGrpNam);
                    if (response.ExecutionOK)
                    {
                        listado = response.Data;
                    }
                    else
                    {
                        this.ShowNotificacion("error", "", response.Message, "4", "0");
                    }
                }
                else
                {
                    this.ShowNotificacion("error", "", "Es necesario que seleccione un Inventario", "4", "0");
                }
            }
            catch (Exception ex)
            {
                this.ShowNotificacion("error", "", ex.Message, "4", "0");
            }

            TempData["DiferenciasConteoUno_Inventarios"] = listaInventarios;
            TempData["DiferenciasConteoUno_Categorias"] = listaCategorias;
            TempData["DiferenciasConteoUno_Listado"] = listado;

            listadoVM.Listado = listado;

            return View(listadoVM);
        }

        public ActionResult ExcelConteoUno()
        {
            var response = new DBResponse<string>();

            try
            {
                List<Listado_DiferenciasConteoUno> listado = (List<Listado_DiferenciasConteoUno>)TempData["DiferenciasConteoUno_Listado"];
                TempData["DiferenciasConteoUno_Listado"] = listado;

                if (listado != null && listado.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("WhsCode");
                    dt.Columns.Add("ItmsGrpName");
                    dt.Columns.Add("ItemCode");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("UOM1");
                    dt.Columns.Add("Onhand");
                    dt.Columns.Add("PorProcesar");
                    dt.Columns.Add("Contado");
                    dt.Columns.Add("DifPesosNeto");
                    dt.Columns.Add("Marbetes");

                    foreach (var item in listado)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.WhsCode;
                        row[1] = item.ItmsGrpNam;
                        row[2] = item.ItemCode;
                        row[3] = item.ItemName;
                        row[4] = item.Uom;
                        row[5] = item.Onhand;
                        row[6] = item.PorProcesar;
                        row[7] = item.Contado;
                        row[8] = item.DifPesoNeto;
                        row[9] = item.Marbetes;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddhhmmss") + "_DiferenciasConteo1.xls";
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Diferencias Conteo 1", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ProcesarFormato()
        {
            var response = new DBResponse<int>();

            var listadoBase = (List<Listado_DiferenciasConteoUno>)TempData["DiferenciasConteoUno_Listado"];
            TempData["DiferenciasConteoUno_Listado"] = listadoBase;

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                decimal monto = decimal.Parse(form["monto"].ToString());
                string categoria = form["categoria"].ToString();

                TempData["DiferenciasConteoUno_IdConfiguracion"] = idConfiguracion;
                TempData["DiferenciasConteoUno_MontoVarianza"] = monto;
                TempData["DiferenciasConteoUno_Categoria"] = categoria;

                response.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public ActionResult Reporte()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_ReporteVM listadoVM = new Diferencias_ReporteVM();
            List<Diferencias_Zonas> listadoZonas = new List<Diferencias_Zonas>();
            List<Diferencias_Reporte> listado = new List<Diferencias_Reporte>();

            var listadoBase = (List<Listado_DiferenciasConteoUno>)TempData["DiferenciasConteoUno_Listado"];
            int idConfiguracion = int.Parse(TempData["DiferenciasConteoUno_IdConfiguracion"].ToString());
            decimal montoVarianza = decimal.Parse(TempData["DiferenciasConteoUno_MontoVarianza"].ToString());
            string categoria = TempData["DiferenciasConteoUno_Categoria"].ToString();

            try
            {
                DBResponse<Configuraciones> responseCon = new ConfiguracionesBL().Search_ConfiguracionByID(idConfiguracion);
                if (responseCon.ExecutionOK)
                {
                    listadoVM.Almacen = responseCon.Data.Almacen;
                    listadoVM.Servidor = responseCon.Data.ServidorIP;
                    listadoVM.Categoria = categoria;
                }

                DBResponse<List<Diferencias_Reporte>> response = new ConfiguracionesBL().Get_DiferenciasReporte(idConfiguracion, montoVarianza, categoria);
                if (response.ExecutionOK)
                {
                    if (response.Data != null && response.Data.Count > 0)
                    {
                        listado = response.Data;

                        List<int> idZonas = listado.Select(s => s.IdZona).Distinct().ToList();
                        foreach (int id in idZonas)
                        {
                            listadoZonas.Add(new Diferencias_Zonas
                            {
                                IdZona = id,
                                Zona = response.Data.Where(w => w.IdZona == id).FirstOrDefault().Zona
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            listadoVM.ListadoZonas = listadoZonas;
            listadoVM.Listado = listado;

            TempData["DiferenciasConteoUno_Listado"] = listadoBase;
            TempData["DiferenciasConteoUno_IdConfiguracion"] = idConfiguracion;
            TempData["DiferenciasConteoUno_MontoVarianza"] = montoVarianza;
            TempData["DiferenciasConteoUno_Categoria"] = categoria;

            return View(listadoVM);
        }


        [HttpPost]
        public ActionResult ProcesarFormatoZonas()
        {
            var response = new DBResponse<int>();

            var listadoBase = (List<Listado_DiferenciasConteoUno>)TempData["DiferenciasConteoUno_Listado"];
            TempData["DiferenciasConteoUno_Listado"] = listadoBase;

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                decimal monto = decimal.Parse(form["monto"].ToString());
                int idZona = int.Parse(form["idZona"].ToString());

                TempData["DiferenciasConteoUno_IdConfiguracion"] = idConfiguracion;
                TempData["DiferenciasConteoUno_MontoVarianza"] = monto;
                TempData["DiferenciasConteoUno_IdZona"] = idZona;

                response.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }


        public ActionResult ReporteZonas()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_ReporteZonasVM listadoVM = new Diferencias_ReporteZonasVM();
            List<string> listaCategorias = new List<string>();
            List<Diferencias_Reporte> listado = new List<Diferencias_Reporte>();

            var listadoBase = (List<Listado_DiferenciasConteoUno>)TempData["DiferenciasConteoUno_Listado"];
            int idConfiguracion = int.Parse(TempData["DiferenciasConteoUno_IdConfiguracion"].ToString());
            decimal montoVarianza = decimal.Parse(TempData["DiferenciasConteoUno_MontoVarianza"].ToString());
            int idZona = int.Parse(TempData["DiferenciasConteoUno_IdZona"].ToString());

            try
            {
                DBResponse<Configuraciones> responseCon = new ConfiguracionesBL().Search_ConfiguracionByID(idConfiguracion);
                if (responseCon.ExecutionOK)
                {
                    listadoVM.Almacen = responseCon.Data.Almacen;
                    listadoVM.Servidor = responseCon.Data.ServidorIP;
                    listadoVM.IdZona = idZona;
                    listadoVM.Zona = new ConfiguracionesBL().Search_ZonaByIdConfiguracionAndIdZona(idConfiguracion, idZona).Data.Zona;
                }

                DBResponse<List<Diferencias_Reporte>> response = new ConfiguracionesBL().Get_DiferenciasReporteZona(idConfiguracion, montoVarianza, idZona);
                if (response.ExecutionOK)
                {
                    if (response.Data != null && response.Data.Count > 0)
                    {
                        listado = response.Data;
                        listaCategorias = listado.Select(s => s.Categoria).Distinct().ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            listadoVM.ListaCategorias = listaCategorias;
            listadoVM.Listado = listado;

            TempData["DiferenciasConteoUno_Listado"] = listadoBase;
            TempData["DiferenciasConteoUno_IdConfiguracion"] = idConfiguracion;
            TempData["DiferenciasConteoUno_MontoVarianza"] = montoVarianza;
            TempData["DiferenciasConteoUno_IdZona"] = idZona;

            return View(listadoVM);
        }

        public ActionResult Ajustes()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_AjustesVM listadoVM = new Diferencias_AjustesVM();
            List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

            listadoVM.FiltroIdConfiguracion = 0;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;
            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;

            listadoVM.FiltroValorItmsGrpNam = "";
            var listaCategorias = new OtrosBL().Get_ListadoCategorias("- Seleccione -").Data;
            listadoVM.FiltroItmsGrpNam = listaCategorias;

            TempData["DiferenciasAjustes_Inventarios"] = listaInventarios;
            TempData["DiferenciasAjustes_Categorias"] = listaCategorias;
            TempData["DiferenciasAjustes_Ajustes"] = null;

            return View(listadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ajustes(Diferencias_AjustesVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Diferencias_AjustesVM listadoVM = new Diferencias_AjustesVM();
            List<Listado_DiferenciasAjustes> listado = new List<Listado_DiferenciasAjustes>();
            List<Listado_AjustesConteoDos> listadoAjustes = new List<Listado_AjustesConteoDos>();

            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;
            var listaInventarios = (List<ControlDDL>)TempData["DiferenciasAjustes_Inventarios"];

            listadoVM.FiltroInventarios = listaInventarios;
            listadoVM.FiltroBusca = viewModel.FiltroBusca;
            listadoVM.FiltroValorItmsGrpNam = viewModel.FiltroValorItmsGrpNam;
            var listaCategorias = (List<ControlDDL2>)TempData["DiferenciasAjustes_Categorias"];
            listadoVM.FiltroItmsGrpNam = listaCategorias;


            if(TempData["DiferenciasAjustes_Listado"] != null)
            {
                listado = (List<Listado_DiferenciasAjustes>)TempData["DiferenciasAjustes_Listado"];
            }

            try
            {
                bool pasaRevision = true;

                if (TempData["DiferenciasAjustes_Ajustes"] != null)
                {
                    pasaRevision = false;

                    List<Listado_AjustesConteoDos> modificados = (List<Listado_AjustesConteoDos>)TempData["DiferenciasAjustes_Ajustes"];
                    if(modificados.Count > 0)
                    {
                        TempData["DiferenciasAjustes_Ajustes"] = modificados;
                        this.ShowNotificacion("error", "", "Hay cambios en la informacion que no se han guardado, reviselos.", "4", "0");
                    }
                    else
                        pasaRevision = true;
                }

                if (pasaRevision)
                {
                    DBResponse<List<Listado_DiferenciasAjustes>> response = new ConfiguracionesBL().Get_AjustesConteoDos(listadoVM.FiltroIdConfiguracion, listadoVM.FiltroBusca, listadoVM.FiltroValorItmsGrpNam);
                    if (response.ExecutionOK)
                    {
                        listado = response.Data;
                    }
                    else
                    {
                        this.ShowNotificacion("error", "", response.Message, "4", "0");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowNotificacion("error", "", ex.Message, "4", "0");
            }

            TempData["DiferenciasAjustes_Inventarios"] = listaInventarios;
            TempData["DiferenciasAjustes_Ajustes"] = listadoAjustes;
            TempData["DiferenciasAjustes_Listado"] = listado;
            TempData["DiferenciasAjustes_Categorias"] = listaCategorias;

            listadoVM.Listado = listado;

            return View(listadoVM);
        }

        public ActionResult ExcelAjustes()
        {
            var response = new DBResponse<string>();

            try
            {
                List<Listado_DiferenciasAjustes> listado = (List<Listado_DiferenciasAjustes>)TempData["DiferenciasAjustes_Listado"];
                TempData["DiferenciasAjustes_Listado"] = listado;

                if (listado != null && listado.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("WhsCode");
                    dt.Columns.Add("ItmsGrpName");
                    dt.Columns.Add("ItemCode");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("UOM1");
                    dt.Columns.Add("Onhand");
                    dt.Columns.Add("PorProcesar");
                    dt.Columns.Add("Contado");
                    dt.Columns.Add("Contado1");
                    dt.Columns.Add("Contado2");
                    dt.Columns.Add("Contado3");
                    dt.Columns.Add("Contado4");
                    dt.Columns.Add("DifPesosNeto");

                    foreach (var item in listado)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.WhsCode;
                        row[1] = item.ItmsGrpNam;
                        row[2] = item.ItemCode;
                        row[3] = item.ItemName;
                        row[4] = item.Uom1;
                        row[5] = item.Onhand;
                        row[6] = item.PorProcesar;
                        row[7] = item.Contado;
                        row[8] = item.Contado1;
                        row[9] = item.Contado2;
                        row[10] = item.Contado3;
                        row[11] = item.Contado4;
                        row[12] = item.DifPesosNeto;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddhhmmss") + "_DiferenciasAjustesConteo2.xls";
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Diferencias Ajustes Conteo 2", nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Actualizar_Conteo2()
        {
            var response = new DBResponse<decimal>();
            decimal totalContadoZonas = 0;

            try
            {
                List<Listado_DiferenciasAjustes> listadoBase = (List<Listado_DiferenciasAjustes>)TempData["DiferenciasAjustes_Listado"];
                List<Listado_AjustesConteoDos> listadoAjustes = (List<Listado_AjustesConteoDos>)TempData["DiferenciasAjustes_Ajustes"];

                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                int idDetalle = int.Parse(form["idDetalle"].ToString());
                int campo = int.Parse(form["campo"].ToString());
                decimal cantidad = decimal.Parse(form["cantidad"].ToString());

                Listado_AjustesConteoDos item = listadoAjustes.Where(w => w.IdDetalle == idDetalle).FirstOrDefault();

                if (item != null)
                {
                    listadoAjustes.Remove(item);

                    if (campo == 1)
                    {
                        item.Contado1 = cantidad;
                        item.Modificado1 = 1;
                    }

                    if (campo == 2)
                    {
                        item.Contado2 = cantidad;
                        item.Modificado2 = 1;
                    }

                    if (campo == 3)
                    {
                        item.Contado3 = cantidad;
                        item.Modificado3 = 1;
                    }

                    if (campo == 4)
                    {
                        item.Contado4 = cantidad;
                        item.Modificado4 = 1;
                    }

                    item.Cantidad = item.Contado1 + item.Contado2 + item.Contado3 + item.Contado4;
                    totalContadoZonas = item.Cantidad;

                    listadoAjustes.Add(item);
                }
                else
                {
                    Listado_DiferenciasAjustes itemOriginal = listadoBase.Where(w => w.IdDetalle == idDetalle).FirstOrDefault();
                    Listado_AjustesConteoDos nuevoItem = new Listado_AjustesConteoDos();
                    nuevoItem.IdDetalle = idDetalle;
                    nuevoItem.Cantidad = itemOriginal.Contado;
                    nuevoItem.Contado1 = itemOriginal.Contado1;
                    nuevoItem.Contado2 = itemOriginal.Contado2;
                    nuevoItem.Contado3 = itemOriginal.Contado3;
                    nuevoItem.Contado4 = itemOriginal.Contado4;

                    if (campo == 1)
                    {
                        nuevoItem.Contado1 = cantidad;
                        nuevoItem.Modificado1 = 1;
                    }

                    if (campo == 2)
                    {
                        nuevoItem.Contado2 = cantidad;
                        nuevoItem.Modificado2 = 1;
                    }

                    if (campo == 3)
                    {
                        nuevoItem.Contado3 = cantidad;
                        nuevoItem.Modificado3 = 1;
                    }

                    if (campo == 4)
                    {
                        nuevoItem.Contado4 = cantidad;
                        nuevoItem.Modificado4 = 1;
                    }

                    totalContadoZonas = nuevoItem.Contado1 + nuevoItem.Contado2 + nuevoItem.Contado3 + nuevoItem.Contado4;
                    nuevoItem.Cantidad = totalContadoZonas;

                    listadoAjustes.Add(nuevoItem);
                }

                TempData["DiferenciasAjustes_Listado"] = listadoBase;
                TempData["DiferenciasAjustes_Ajustes"] = listadoAjustes;
                response.ExecutionOK = true;
                response.Data = totalContadoZonas;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Aplicar_AjustesConteo2()
        {
            var response = new DBResponse<int>();
            List<Listado_DiferenciasAjustes> listado = (List<Listado_DiferenciasAjustes>)TempData["DiferenciasAjustes_Listado"];
            List<Listado_AjustesConteoDos> listadoAjustes = (List<Listado_AjustesConteoDos>)TempData["DiferenciasAjustes_Ajustes"];

            var listaCategorias = (List<ControlDDL2>)TempData["DiferenciasAjustes_Categorias"];
            var listaInventarios = (List<ControlDDL>)TempData["DiferenciasAjustes_Inventarios"];
            TempData["DiferenciasAjustes_Inventarios"] = listaInventarios;
            TempData["DiferenciasAjustes_Categorias"] = listaCategorias;

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());

                response = new ConfiguracionesBL().Update_AjustesConteoDos(idConfiguracion, listadoAjustes);
            }
            catch (Exception ex)
            {
                TempData["DiferenciasAjustes_Listado"] = listado;
                TempData["DiferenciasAjustes_Ajustes"] = listadoAjustes;

                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ExcelCostos(int idConfiguracion)
        {
            var response = new DBResponse<string>();

            try
            {
                // Obtener los datos directamente de la base de datos
                var resultado = new ConfiguracionesBL().Get_CostoCeroInactivos(idConfiguracion);

                if (resultado.ExecutionOK && resultado.Data != null && resultado.Data.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ItemCode");
                    dt.Columns.Add("Descripcion");
                    dt.Columns.Add("Estatus");

                    foreach (var item in resultado.Data)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = item.ItemCode;
                        row[1] = item.Descripcion;
                        row[2] = item.Estatus;
                        dt.Rows.Add(row);
                    }

                    string nombreArchivo = DateTime.Now.ToString("yyyyMMddHHmmss") + "_CostosCeroInactivos.xls";

                    // ===== LLAMAR AL MÉTODO QUE GUARDA EL EXCEL =====
                    response = ExportExcel.GrabaArchivoExcelSimple(dt, "Costos Cero Inactivos", nombreArchivo);
                    // ================================================

                    // Si el método no estableció ExecutionOK, lo hacemos aquí
                    if (response.Data != null)
                    {
                        response.ExecutionOK = true;
                    }
                }
                else
                {
                    response.ExecutionOK = false;
                    response.Message = "No hay datos para exportar.";
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = ex.Message;
            }

            return Json(response);
        }
    }  // <-- Esta es la llave de cierre de la clase
}