using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Entities.Generics;
using Blanquita_Inventarios.Site.Helpers;
using Blanquita_Inventarios.Site.Models;
using Blanquita_Inventarios.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Blanquita_Inventarios.Site.Controllers
{
    public class ConfiguracionesController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 1 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            return View(Get_Configuraciones(new Listado_ConfiguracionesVM()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Listado_ConfiguracionesVM viewModel)
        {
            return View(Get_Configuraciones(viewModel));
        }

        public ActionResult RestablecerConfiguraciones()
        {
            return RedirectToAction("Index");
        }

        private Listado_ConfiguracionesVM Get_Configuraciones(Listado_ConfiguracionesVM viewModel)
        {
            Listado_ConfiguracionesVM listadoVM = new Listado_ConfiguracionesVM();

            TempData["messages"] = new Dictionary<string, string[]>();

            List<Listado_Configuraciones> listado = new List<Listado_Configuraciones>();

            string fechaActual = string.Format("{0} - {1}", new DateTime(2025, 1, 1).ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            listadoVM.FiltroFechas = !String.IsNullOrEmpty(viewModel.FiltroFechas) ? viewModel.FiltroFechas : fechaActual;
            listadoVM.FiltroInventario = viewModel.FiltroInventario;
            listadoVM.FiltroServidor = viewModel.FiltroServidor;

            string fechas = listadoVM.FiltroFechas;
            DateTime fechaInicial = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"), new CultureInfo("es-MX"));
            DateTime fechaFinal = DateTime.Now.AddDays(1).AddMinutes(-1);
            string fIni = "";
            string fFin = "";

            if (!String.IsNullOrEmpty(listadoVM.FiltroFechas))
            {
                fIni = fechas.Split('-')[0].Trim();
                fFin = fechas.Split('-')[1].Trim();

                fechaInicial = DateTime.Parse(fIni, new CultureInfo("es-MX"));
                fechaFinal = DateTime.Parse(fFin, new CultureInfo("es-MX")).AddDays(1).AddMinutes(-1);
            }

            DBResponse<List<Configuraciones>> response = new ConfiguracionesBL().Get_Configuraciones(
                fechaInicial,
                fechaFinal,
                listadoVM.FiltroInventario,
                listadoVM.FiltroServidor);
            if (response.ExecutionOK)
            {

                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (Configuraciones i in response.Data)
                    {
                        listado.Add(new Listado_Configuraciones()
                        {
                            IdConfiguracion = i.IdConfiguracion,
                            FechaRegistro = i.FechaRegistro,
                            NombreInventario = i.NombreInventario,
                            Servidor = i.ServidorIP,
                            NombreInstancia = i.Instancia,
                            Usuario = i.Usuario,
                            Password = i.Password,
                            BaseDatos = i.BaseDatos,
                            Almacen = i.Almacen,
                            Cerrado = i.Cerrado,
                            Deshabilitado = i.Deshabilitado
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

            ConfiguracionVM viewModel = new ConfiguracionVM();
            viewModel.IdConfiguracion = 0;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(ConfiguracionVM viewModel)
        {
            DBResponse<int> result = new DBResponse<int>();

            if (ModelState.IsValid)
            {
                DBResponse<Configuraciones> response = new ConfiguracionesBL().Upsert_Configuracion(
                   new Configuraciones()
                   {
                       IdConfiguracion = 0,
                       FechaRegistro = DateTime.Now,
                       NombreInventario = viewModel.NombreInventario,
                       ServidorIP = viewModel.Servidor != null ? viewModel.Servidor : "",
                       Instancia = viewModel.NombreInstancia != null ? viewModel.NombreInstancia : "",
                       Puerto = viewModel.Puerto != null ? viewModel.Puerto : "",
                       Usuario = viewModel.Usuario,
                       Password = viewModel.Password,
                       BaseDatos = viewModel.BaseDatos,
                       Almacen = viewModel.Almacen,
                       Cerrado = false,
                       Deshabilitado = false,
                       Activo = true
                   },
                   new ConexSAP
                   {
                       Server = Config.SAPHostname,
                       Puerto = Config.SAPPuerto,
                       User = Config.SAPUser,
                       Password = Config.SAPPwd,
                       Schema = Config.SAPSchema
                   });

                result.ExecutionOK = response.ExecutionOK;
                result.Message = response.Message;       
                
                if(response.ExecutionOK)
                    TempData["MensajeAIndex"] = "Se ha guardado correctamente los datos de la Configuración del Servidor";
            }
            else
            {
                result.Message = "Verificar errores. <br /> Asegúrese de haber ingresado los datos correspondientes a: <br /> - Nombre de Inventario <br /> - Servidor (IP) <br /> - Usuario <br /> - Password <br /> - Base de Datos <br /> - Almacén";                
            }

            //do work
            return Json(result);//this can be an object if you need to return more data
        }

        public ActionResult Details(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return RedirectToAction("Index", "Configuraciones");

            TempData["messages"] = new Dictionary<string, string[]>();

            ConfiguracionVM viewModel = new ConfiguracionVM();
            viewModel.IdConfiguracion = 0;

            DBResponse<Configuraciones> response = new ConfiguracionesBL().Search_ConfiguracionByID(id.Value);
            if (response.ExecutionOK)
            {
                viewModel.IdConfiguracion = id.Value;
                viewModel.FechaRegistro = response.Data.FechaRegistro;
                viewModel.NombreInventario = response.Data.NombreInventario;
                viewModel.Servidor = response.Data.ServidorIP;
                viewModel.NombreInstancia = response.Data.Instancia;
                viewModel.Puerto = response.Data.Puerto;
                viewModel.Usuario = response.Data.Usuario;
                viewModel.Password = response.Data.Password;
                viewModel.BaseDatos = response.Data.BaseDatos;
                viewModel.Almacen = response.Data.Almacen;
            }
            else
            {
                TempData["MensajeAIndex"] = response.Message;
                return RedirectToAction("Index", "Configuraciones");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(ConfiguracionVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (ModelState.IsValid)
            {
                DBResponse<Configuraciones> response = new ConfiguracionesBL().Upsert_Configuracion(
                   new Configuraciones()
                   {
                       IdConfiguracion = viewModel.IdConfiguracion,
                       NombreInventario = viewModel.NombreInventario,
                       ServidorIP = viewModel.Servidor,
                       Instancia = String.IsNullOrEmpty(viewModel.NombreInstancia) ? "" : viewModel.NombreInstancia,
                       Puerto = viewModel.Puerto,
                       Usuario = viewModel.Usuario,
                       Password = viewModel.Password,
                       BaseDatos = viewModel.BaseDatos,
                       Almacen = viewModel.Almacen
                   }, new ConexSAP());

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

            TempData["MensajeAIndex"] = "Se ha guardado correctamente los datos de la Configuración del Servidor";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerarBackup()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                string directorioBackups = Config.DirectorioDocumentos + "\\Backups";

                response = new ConfiguracionesBL().Generate_BackupInventario(idConfiguracion, directorioBackups);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarInventario(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<Configuraciones> response = new ConfiguracionesBL().Cerrar_Configuracion(id.Value);
            if (response.ExecutionOK)
            {
                TempData["MensajeAIndex"] = "El Inventario se cerro correctamente.";
            }
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeshabilitarInventario(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<Configuraciones> response = new ConfiguracionesBL().Deshabilitar_Configuracion(id.Value);
            if (response.ExecutionOK)
            {
                TempData["MensajeAIndex"] = "El Inventario se deshabilito correctamente.";
            }
            else
                TempData["MensajeAIndex"] = response.Message;

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
            DBResponse<Configuraciones> response = new ConfiguracionesBL().Delete_Configuracion(id.Value);
            if (response.ExecutionOK)
            {
                TempData["MensajeAIndex"] = "La Configuración del Servidor se ha eliminado correctamente.";
            }
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }


        #region Zonas

        public ActionResult Zonas(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 1 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            return View(Get_Zonas(new Listado_ZonasVM { IdConfiguracion = id.Value }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Zonas(Listado_ZonasVM viewModel)
        {
            return View(Get_Zonas(viewModel));
        }

        public ActionResult RestablecerZonas(int? id)
        {
            return RedirectToAction("Zonas", new { id = id });
        }


        private Listado_ZonasVM Get_Zonas(Listado_ZonasVM viewModel)
        {
            Listado_ZonasVM listadoVM = new Listado_ZonasVM();

            TempData["messages"] = new Dictionary<string, string[]>();

            List<Listado_Zonas> listado = new List<Listado_Zonas>();

            string fechaActual = string.Format("{0} - {1}", new DateTime(2025, 1, 1).ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            listadoVM.IdConfiguracion = viewModel.IdConfiguracion;
            listadoVM.FiltroFechas = !String.IsNullOrEmpty(viewModel.FiltroFechas) ? viewModel.FiltroFechas : fechaActual;
            listadoVM.FiltroZona = viewModel.FiltroZona;

            string fechas = listadoVM.FiltroFechas;
            DateTime fechaInicial = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"), new CultureInfo("es-MX"));
            DateTime fechaFinal = DateTime.Now.AddDays(1).AddMinutes(-1);
            string fIni = "";
            string fFin = "";

            if (!String.IsNullOrEmpty(listadoVM.FiltroFechas))
            {
                fIni = fechas.Split('-')[0].Trim();
                fFin = fechas.Split('-')[1].Trim();

                fechaInicial = DateTime.Parse(fIni, new CultureInfo("es-MX"));
                fechaFinal = DateTime.Parse(fFin, new CultureInfo("es-MX")).AddDays(1).AddMinutes(-1);
            }

            DBResponse<List<Zonas>> response = new ConfiguracionesBL().Get_Zonas(
                listadoVM.IdConfiguracion,
                fechaInicial,
                fechaFinal,
                listadoVM.FiltroZona);
            if (response.ExecutionOK)
            {

                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (Zonas i in response.Data)
                    {
                        listado.Add(new Listado_Zonas()
                        {
                            IdZona = i.IdZona,
                            FechaRegistro = i.FechaRegistro,
                            Zona = i.Zona,
                            MarbeteInicial = i.MarbeteInicial,
                            MarbeteFinal = i.MarbeteFinal,
                            MarbetesCreados = i.MarbetesCreados
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

        public ActionResult CreateZona(int? idConfiguracion)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            ZonaVM viewModel = new ZonaVM();
            viewModel.IdConfiguracion = idConfiguracion.Value;
            viewModel.HabilitarMarbeteInicial = true;

            //Verificamos zonas para ver si hay que deshabilitar el campo marbete inicial
            try
            {
                var responseZona = new ConfiguracionesBL().Search_ZonaMarbeteInicial(idConfiguracion.Value);
                if (responseZona.ExecutionOK)
                {
                    if (responseZona.Data != null)
                    {
                        viewModel.HabilitarMarbeteInicial = false;
                        viewModel.MarbeteInicial = responseZona.Data.MarbeteFinal + 1;
                    }
                }
                else
                {
                    TempData["MensajeAIndex"] = responseZona.Message;
                    return RedirectToAction("Zonas", new { id = viewModel.IdConfiguracion });
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeAIndex"] = ex.Message;
                return RedirectToAction("Zonas", new { id = viewModel.IdConfiguracion });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateZona(ZonaVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (ModelState.IsValid)
            {
                DBResponse<Zonas> response = new ConfiguracionesBL().Insert_Zona(
                   new Zonas()
                   {
                       IdConfiguracion = viewModel.IdConfiguracion,
                       Zona = viewModel.Zona,
                       MarbeteInicial = viewModel.MarbeteInicial,
                       MarbeteFinal = viewModel.MarbeteFinal
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

            TempData["MensajeAIndex"] = "Se ha guardado correctamente los datos de la Zona";
            return RedirectToAction("Zonas", new { id = viewModel.IdConfiguracion });
        }

        [HttpPost]
        public ActionResult CreateMarbetes()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idZona = int.Parse(form["idZona"].ToString());

                DBResponse<Zonas> responseZonas = new ConfiguracionesBL().Create_ZonaMarbetes(idZona);
                if (responseZonas.ExecutionOK)
                {
                    response.ExecutionOK = true;

                    TempData["MensajeAIndex"] = "Los Marbetes se crearon correctamente";
                }
                else
                {
                    response.Message = responseZonas.Message;
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteMarbetes()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idZona = int.Parse(form["idZona"].ToString());

                DBResponse<Zonas> responseZonas = new ConfiguracionesBL().Delete_ZonaMarbetes(idZona);
                if (responseZonas.ExecutionOK)
                {
                    response.ExecutionOK = true;
                    response.Message = "Los Marbetes se eliminaron correctamente";
                }
                else
                {
                    response.Message = responseZonas.Message;
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteZona()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idZona = int.Parse(form["idZona"].ToString());

                DBResponse<Zonas> responseZonas = new ConfiguracionesBL().Delete_Zona(idZona);
                if (responseZonas.ExecutionOK)
                {
                    response.ExecutionOK = true;
                    response.Message = "La Zona se elimino correctamente";
                }
                else
                {
                    response.Message = responseZonas.Message;
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return Json(response);
        }

        #endregion

        #region Codigos 

        public ActionResult Codigos(int? id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Index", "Home");

            UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
            bool isAdmin = (userLogin.IdPerfil == 1 || userLogin.IdPerfil == 4) ? true : false;
            if (!isAdmin)
                return RedirectToAction("Home", "Home");

            return View(Get_Codigos(new Listado_CodigosVM { IdConfiguracion = id.Value }));
        }

        private Listado_CodigosVM Get_Codigos(Listado_CodigosVM viewModel)
        {
            Listado_CodigosVM listadoVM = new Listado_CodigosVM();

            TempData["messages"] = new Dictionary<string, string[]>();

            List<Listado_Codigos> listado = new List<Listado_Codigos>();

            listadoVM.IdConfiguracion = viewModel.IdConfiguracion;

            DBResponse<List<Codigos>> response = new ConfiguracionesBL().Get_CodigosByConfiguracion(
                listadoVM.IdConfiguracion);
            if (response.ExecutionOK)
            {

                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (Codigos i in response.Data)
                    {
                        listado.Add(new Listado_Codigos()
                        {
                            Codigo = i.Codigo,
                            FechaGeneracion = i.FechaGeneracion,
                            UsuarioGenero = i.Usuarios.Nombre,
                            UsuarioUtilizo = i.Usuarios1 != null ? i.Usuarios1.Nombre : "",
                            Accion = i.AccionesCodigo != null ? i.AccionesCodigo.Accion : "",
                            Estatus = i.EstatusCodigo != null ? i.EstatusCodigo.Estatus : ""
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

        [HttpPost]
        public ActionResult GenerarCodigo()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());

                UsuarioSesion userLogin = (UsuarioSesion)Session["UserAdmin"];
                string codigo = Utils.CreateCode(4);
                DBResponse<Codigos> query = new ConfiguracionesBL().Insert_Codigo(
                    new Codigos
                    {
                        IdConfiguracion = idConfiguracion,
                        Codigo = codigo,
                        FechaGeneracion = DateTime.Now,
                        IdUsuarioGenero = userLogin.Id,
                        IdEstatus = 1
                    });
                if (query.ExecutionOK)
                {
                    TempData["MensajeAIndex"] = "Se genero el código correctamente.";
                    response.ExecutionOK = true;
                }
                else
                {
                    response.Message = query.Message;
                }

            }
            catch (Exception ex)
            {
                string msjError = ex.Message;
                if (ex.InnerException != null)
                    msjError += ex.InnerException.Message;
            }

            return Json(response);
        }

        #endregion

        #region Impresion Marbetes

        [HttpPost]
        public ActionResult ProcesarFormato()
        {
            var response = new DBResponse<int>();

            try
            {
                var form = Request.Form;
                int idConfiguracion = int.Parse(form["idConfiguracion"].ToString());
                int inicio = int.Parse(form["inicio"].ToString());
                int fin = int.Parse(form["fin"].ToString());

                DBResponse<Configuraciones> query = new ConfiguracionesBL().Search_ConfiguracionByID(idConfiguracion);
                if (query.ExecutionOK)
                {
                    TempData["ImpresionMarbetes"] = new ImpresionMarbetesVM
                    {
                        Almacen = query.Data.Almacen,
                        Inicio = inicio,
                        Fin = fin
                    };

                    response.ExecutionOK = true;
                }
                else
                {
                    response.Message = query.Message;
                }

            }
            catch (Exception ex)
            {
                string msjError = ex.Message;
                if (ex.InnerException != null)
                    msjError += ex.InnerException.Message;
            }

            return Json(response);
        }

        public ActionResult FormatoUno()
        {
            ImpresionMarbetesVM viewModel = (ImpresionMarbetesVM)TempData["ImpresionMarbetes"];
            TempData["ImpresionMarbetes"] = viewModel;

            try
            {
                string filePath = Config.DirectorioDocumentos + "\\Codigos\\";

                for (int i = viewModel.Inicio; i <= viewModel.Fin; i++)
                {
                    ZXing.BarcodeWriter writer = new ZXing.BarcodeWriter();
                    writer.Format = ZXing.BarcodeFormat.CODE_128;
                    writer.Options.Height = 80;
                    writer.Options.Margin = 5;
                    writer.Options.PureBarcode = false;

                    var image = writer.Write(i.ToString());
                    image.Save(filePath + i.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {

            }

            return View(viewModel);
        }

        public ActionResult FormatoDos()
        {
            ImpresionMarbetesVM viewModel = (ImpresionMarbetesVM)TempData["ImpresionMarbetes"];
            TempData["ImpresionMarbetes"] = viewModel;

            return View(viewModel);
        }

        #endregion
    }
}