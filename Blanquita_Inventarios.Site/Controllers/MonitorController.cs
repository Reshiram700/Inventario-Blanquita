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
using System.Web.SessionState;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class MonitorController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Avance_ZonasVM listadoVM = new Avance_ZonasVM();
            List<Listado_AvanceZona> listado = new List<Listado_AvanceZona>();

            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;
            TempData["AvanceZona_Inventarios"] = listaInventarios;
            listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;

            Avance_Zona avance = new Avance_Zona();
            avance.Avance = 0;
            avance.Monto = 0;
            avance.Tiempo = "0";
            avance.Inventario = "";
            avance.Almacen = "";

            avance.Listado = listado;
            listadoVM.Avance = avance;

            return View(listadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Avance_ZonasVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Avance_ZonasVM listadoVM = new Avance_ZonasVM();
            List<Listado_AvanceZona> listado = new List<Listado_AvanceZona>();

            listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;

            var listaInventarios = (List<ControlDDL>)TempData["AvanceZona_Inventarios"];
            listadoVM.FiltroInventarios = listaInventarios;
            TempData["AvanceZona_Inventarios"] = listaInventarios;

            try
            {
                Avance_Zona avance = new Avance_Zona();
                avance.Avance = 0;
                avance.Monto = 0;
                avance.Tiempo = "0";
                avance.Inventario = "";
                avance.Almacen = "";

                DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(listadoVM.FiltroIdConfiguracion);
                if (response.ExecutionOK)
                {
                    avance.Inventario = response.Data.NombreInventario;
                    avance.Almacen = response.Data.Almacen;

                    DBResponse<List<Listado_AvanceZona>> responseInformacion = new ConfiguracionesBL().Get_MonitorAvanceZona(listadoVM.FiltroIdConfiguracion);
                    if (responseInformacion.ExecutionOK)
                    {
                        listado = responseInformacion.Data;
                        int marbetes = listado.Sum(s => s.TotalMarbetes);
                        int cerrados = listado.Sum(s => s.Cerrados);
                        int pendientes = listado.Sum(s => s.MarbetesPorCapturar);

                        avance.Avance = (cerrados * 100) / marbetes;
                        avance.Monto = listado.Sum(s => s.Monto);

                        int minutos = listado.Max(s => s.Minutos);
                        int nuevosMinutos = minutos;
                        int horas = minutos / 60;
                        if (horas >= 1)
                        {
                            nuevosMinutos = minutos - (horas * 60);
                        }

                        avance.Tiempo = horas.ToString().PadLeft(2, '0') + ":" + nuevosMinutos.ToString().PadLeft(2, '0');
                    }
                }

                avance.Listado = listado;
                listadoVM.Avance = avance;
            }
            catch (Exception ex)
            {

            }

            return View(listadoVM);
        }

        [HttpPost]
        public ActionResult Ver_Marbetes()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                int idZona = int.Parse(form["idZona"].ToString());

                TempData["Monitor_ZonasIdConfiguracion"] = idConfiguracion;
                TempData["Monitor_ZonasIdZona"] = idZona;

                response.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public ActionResult Marbetes()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Avance_MarbetesVM listadoVM = new Avance_MarbetesVM();
            Avance_Marbetes avance = new Avance_Marbetes();
            List<Listado_AvanceMarbete> listado = new List<Listado_AvanceMarbete>();

            try
            {
                listadoVM.FiltroIdConfiguracion = 0;
                var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
                listadoVM.FiltroInventarios = listaInventarios;
                TempData["AvanceMarbetes_Inventarios"] = listaInventarios;

                if (TempData["Monitor_ZonasIdConfiguracion"] != null)
                    listadoVM.FiltroIdConfiguracion = int.Parse(TempData["Monitor_ZonasIdConfiguracion"].ToString());
                else
                    listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;

                var listaZonas = new OtrosBL().Get_ListadoZonasByConfiguracion(listadoVM.FiltroIdConfiguracion, "- Seleccione -").Data;
                listadoVM.FiltroZonas = listaZonas;

                if (TempData["Monitor_ZonasIdZona"] != null)
                    listadoVM.FiltroIdZona = int.Parse(TempData["Monitor_ZonasIdZona"].ToString());

                TempData["AvanceMarbetes_Zonas"] = listaZonas;

                avance.Avance = 0;
                avance.Monto = 0;
                avance.Tiempo = "00:00";
                avance.Inventario = "";
                avance.Almacen = "";

                if (listadoVM.FiltroIdConfiguracion > 0 && listadoVM.FiltroIdZona > 0)
                {
                    DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(listadoVM.FiltroIdConfiguracion);
                    if (response.ExecutionOK)
                    {
                        avance.Inventario = response.Data.NombreInventario;
                        avance.Almacen = response.Data.Almacen;

                        DBResponse<List<Listado_AvanceMarbete>> responseInformacion = new ConfiguracionesBL().Get_MonitorAvanceMarbete(listadoVM.FiltroIdConfiguracion, listadoVM.FiltroIdZona, listadoVM.FiltroMarbete);
                        if (responseInformacion.ExecutionOK)
                        {
                            listado = responseInformacion.Data;
                            int marbetes = listado.Count;
                            int cerrados = listado.Where(w => w.IdEstatus > 1).ToList().Count;
                            int pendientes = listado.Where(w => w.IdEstatus == 1).ToList().Count;
                            int minutos = listado.Sum(s => s.TotalMinutos);

                            avance.Avance = (cerrados * 100) / marbetes;
                            avance.Monto = listado.Sum(s => s.Importe);

                            int nuevosMinutos = minutos;
                            int horas = minutos / 60;
                            if (horas >= 1)
                            {
                                nuevosMinutos = minutos - (horas * 60);
                            }

                            avance.Tiempo = horas.ToString().PadLeft(2, '0') + ":" + nuevosMinutos.ToString().PadLeft(2, '0');
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

            avance.Listado = listado;
            listadoVM.Avance = avance;

            return View(listadoVM);
        }

        [HttpPost]
        public ActionResult Get_ZonasByConfiguracion(string idConfiguracion)
        {
            var listaZonas = new OtrosBL().Get_ListadoZonasByConfiguracion(int.Parse(idConfiguracion), "- Seleccione -").Data;
            TempData["AvanceMarbetes_Zonas"] = listaZonas;
            return Json(listaZonas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Marbetes(Avance_MarbetesVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Avance_MarbetesVM listadoVM = new Avance_MarbetesVM();
            Avance_Marbetes avance = new Avance_Marbetes();
            List<Listado_AvanceMarbete> listado = new List<Listado_AvanceMarbete>();

            try
            {
                listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;

                var listaInventarios = (List<ControlDDL>)TempData["AvanceMarbetes_Inventarios"];
                listadoVM.FiltroInventarios = listaInventarios;
                TempData["AvanceMarbetes_Inventarios"] = listaInventarios;
                listadoVM.FiltroIdZona = viewModel.FiltroIdZona;

                var listaZonas = (List<ControlDDL>)TempData["AvanceMarbetes_Zonas"];
                listadoVM.FiltroZonas = listaZonas;
                TempData["AvanceMarbetes_Zonas"] = listaZonas;
                listadoVM.FiltroMarbete = viewModel.FiltroMarbete;

                avance.Avance = 0;
                avance.Monto = 0;
                avance.Tiempo = "0";
                avance.Inventario = "";
                avance.Almacen = "";

                DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(listadoVM.FiltroIdConfiguracion);
                if (response.ExecutionOK)
                {
                    avance.Inventario = response.Data.NombreInventario;
                    avance.Almacen = response.Data.Almacen;

                    DBResponse<List<Listado_AvanceMarbete>> responseInformacion = new ConfiguracionesBL().Get_MonitorAvanceMarbete(viewModel.FiltroIdConfiguracion, viewModel.FiltroIdZona, viewModel.FiltroMarbete);
                    if (responseInformacion.ExecutionOK)
                    {
                        listado = responseInformacion.Data;
                        int marbetes = listado.Count;
                        int cerrados = listado.Where(w => w.IdEstatus > 1).ToList().Count;
                        int pendientes = listado.Where(w => w.IdEstatus == 1).ToList().Count;
                        int minutos = listado.Sum(s => s.TotalMinutos);

                        avance.Avance = (cerrados * 100) / marbetes;
                        avance.Monto = listado.Sum(s => s.Importe);

                        int nuevosMinutos = minutos;
                        int horas = minutos / 60;
                        if (horas >= 1)
                        {
                            nuevosMinutos = minutos - (horas * 60);
                        }

                        avance.Tiempo = horas.ToString().PadLeft(2, '0') + ":" + nuevosMinutos.ToString().PadLeft(2, '0');
                    }
                }
            }
            catch (Exception ex)
            {

            }

            avance.Listado = listado;
            listadoVM.Avance = avance;

            return View(listadoVM);
        }

        [HttpPost]
        public ActionResult Ver_Marbete()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                int marbete = int.Parse(form["marbete"].ToString());

                TempData["Monitor_MarbetesIdConfiguracion"] = idConfiguracion;
                TempData["Monitor_MarbetesMarbete"] = marbete;

                response.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        public ActionResult Marbete()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 3 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            TempData["messages"] = new Dictionary<string, string[]>();

            Avance_DetalleMarbeteVM listadoVM = new Avance_DetalleMarbeteVM();
            Avance_DetalleMarbete avance = new Avance_DetalleMarbete();
            List<Listado_AvanceDetalleMarbete> listado = new List<Listado_AvanceDetalleMarbete>();

            listadoVM.FiltroIdConfiguracion = TempData["AvanceDetalleMarbete_IdConfiguracion"] != null ? int.Parse(TempData["AvanceDetalleMarbete_IdConfiguracion"].ToString()) : 0;
            var listaInventarios = new OtrosBL().Get_ListadoInventarios("- Seleccione -").Data;
            listadoVM.FiltroInventarios = listaInventarios;
            TempData["AvanceDetalleMarbete_Inventarios"] = listaInventarios;

            listadoVM.FiltroMarbete = TempData["AvanceDetalleMarbete_Marbete"] != null ? int.Parse(TempData["AvanceDetalleMarbete_Marbete"].ToString()) : 0;

            avance.Inventario = "";
            avance.Almacen = "";

            try
            {
                if (TempData["Monitor_MarbetesIdConfiguracion"] != null)
                    listadoVM.FiltroIdConfiguracion = int.Parse(TempData["Monitor_MarbetesIdConfiguracion"].ToString());
                else
                    listadoVM.FiltroIdConfiguracion = userLogin.IdConfiguracion;

                if (TempData["Monitor_MarbetesMarbete"] != null)
                    listadoVM.FiltroMarbete = int.Parse(TempData["Monitor_MarbetesMarbete"].ToString());

                if (listadoVM.FiltroIdConfiguracion > 0 && listadoVM.FiltroMarbete > 0)
                {
                    DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(listadoVM.FiltroIdConfiguracion);
                    if (response.ExecutionOK)
                    {
                        avance.IdConfiguracion = response.Data.IdConfiguracion;
                        avance.Inventario = response.Data.NombreInventario;
                        avance.Almacen = response.Data.Almacen;
                        avance.Marbete = listadoVM.FiltroMarbete;

                        DBResponse<List<Listado_AvanceDetalleMarbete>> responseInformacion = new ConfiguracionesBL().Get_MonitorAvanceDetalleMarbete(listadoVM.FiltroIdConfiguracion, listadoVM.FiltroMarbete);
                        if (responseInformacion.ExecutionOK)
                        {
                            listado = responseInformacion.Data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            avance.Listado = listado;
            listadoVM.Avance = avance;

            return View(listadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Marbete(Avance_DetalleMarbeteVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            Avance_DetalleMarbeteVM listadoVM = new Avance_DetalleMarbeteVM();
            Avance_DetalleMarbete avance = new Avance_DetalleMarbete();
            List<Listado_AvanceDetalleMarbete> listado = new List<Listado_AvanceDetalleMarbete>();

            try
            {
                listadoVM.FiltroIdConfiguracion = viewModel.FiltroIdConfiguracion;
                listadoVM.FiltroMarbete = viewModel.FiltroMarbete;

                var listaInventarios = (List<ControlDDL>)TempData["AvanceDetalleMarbete_Inventarios"];
                listadoVM.FiltroInventarios = listaInventarios;
                TempData["AvanceDetalleMarbete_Inventarios"] = listaInventarios;

                avance.IdConfiguracion = 0;
                avance.Inventario = "";
                avance.Almacen = "";
                avance.IdMarbete = 0;
                avance.Marbete = 0;

                if (viewModel.FiltroIdConfiguracion == 0 || viewModel.FiltroMarbete == 0)
                {
                    this.ShowNotificacion("error", "", "Asegúrese de haber seleccionado un Inventario y haber ingresado un Marbete", "4", "0");
                }
                else
                {
                    DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(listadoVM.FiltroIdConfiguracion);
                    if (response.ExecutionOK)
                    {
                        avance.IdConfiguracion = response.Data.IdConfiguracion;
                        avance.Inventario = response.Data.NombreInventario;
                        avance.Almacen = response.Data.Almacen;
                        avance.Marbete = viewModel.FiltroMarbete;

                        DBResponse<List<Listado_AvanceDetalleMarbete>> responseInformacion = new ConfiguracionesBL().Get_MonitorAvanceDetalleMarbete(viewModel.FiltroIdConfiguracion, viewModel.FiltroMarbete);
                        if (responseInformacion.ExecutionOK)
                        {
                            listado = responseInformacion.Data;
                        }
                    }
                }

                avance.Listado = listado;
                listadoVM.Avance = avance;
            }
            catch (Exception ex)
            {

            }

            return View(listadoVM);
        }

        [HttpPost]
        public ActionResult Buscar_ProductoByCodigo()
        {
            var response = new DBResponse<DatosBarcode>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                string codigo = form["codigo"].ToString();

                DBResponse<DatosBarcode> responseCodigo = new ConfiguracionesBL().Get_DatosProducto(idConfiguracion, codigo);
                if (responseCodigo.ExecutionOK)
                {
                    response.ExecutionOK = true;
                    response.Data = responseCodigo.Data;
                }
                else
                {
                    response.Message = responseCodigo.Message;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Guardar_Producto()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                string codigo = form["codigo"].ToString();
                decimal cantidad = decimal.Parse(form["cantidad"].ToString());
                int marbete = int.Parse(form["marbete"].ToString());

                UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
                string usuario = userLogin.Usuario;

                response = new ConfiguracionesBL().Insert_Producto(idConfiguracion, marbete, codigo, cantidad, usuario);
                if (response.ExecutionOK)
                {
                    TempData["AvanceDetalleMarbete_IdConfiguracion"] = idConfiguracion;
                    TempData["AvanceDetalleMarbete_Marbete"] = marbete;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Actualizar_CantidadProducto()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                int idDetalle = int.Parse(form["idDetalle"].ToString());
                decimal cantidad = decimal.Parse(form["cantidad"].ToString());
                int marbete = int.Parse(form["marbete"].ToString());

                UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
                string usuario = userLogin.Usuario;

                response = new ConfiguracionesBL().Update_CantidadProducto(idConfiguracion, idDetalle, cantidad);
                if (response.ExecutionOK)
                {
                    TempData["AvanceDetalleMarbete_IdConfiguracion"] = idConfiguracion;
                    TempData["AvanceDetalleMarbete_Marbete"] = marbete;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_DetalleMarbete(int? idConfiguracion, int? idDetalle, int? marbete)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<int> response = new ConfiguracionesBL().Delete_ArticuloMarbete(idConfiguracion.Value, idDetalle.Value);
            if (response.ExecutionOK)
            {
                TempData["MensajeAIndex"] = "Se elimino corretamente el Detalle.";
            }
            else
                TempData["MensajeAIndex"] = response.Message;


            TempData["AvanceDetalleMarbete_IdConfiguracion"] = idConfiguracion;
            TempData["AvanceDetalleMarbete_Marbete"] = marbete.Value;

            return RedirectToAction("Marbete");
        }

        public ActionResult Reporte(int? idConfiguracion, int? marbete)
        {
            Reporte01VM viewModel = new Reporte01VM();
            List<DatosBarcode> listado = new List<DatosBarcode>();

            try
            {
                DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(idConfiguracion.Value);
                if (response.ExecutionOK)
                {
                    viewModel.Inventario = response.Data.NombreInventario;
                    viewModel.Almacen = response.Data.Almacen;
                    viewModel.Servidor = response.Data.ServidorIP;

                    DBResponse<List<DatosBarcode>> responseDatos = new ConfiguracionesBL().Get_MarbeteProductos(idConfiguracion.Value, marbete.Value);
                    if (responseDatos.ExecutionOK)
                    {
                        viewModel.Marbete = responseDatos.Data.Select(s => s.Marbete).FirstOrDefault();
                        viewModel.Capturo = responseDatos.Data.Select(s => s.NombrePDA).FirstOrDefault();
                        listado = responseDatos.Data;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            viewModel.Listado = listado;

            return View(viewModel);
        }
    }
}