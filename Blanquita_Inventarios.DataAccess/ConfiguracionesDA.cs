using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Entities.Generics;
using Sap.Data.Hana;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Blanquita_Inventarios.Site.Helpers;
using System.IO;

namespace Blanquita_Inventarios.DataAccess
{
    public class ConfiguracionesDA
    {
        public DBResponse<List<Configuraciones>> Get_Configuraciones(DateTime fechaInicial, DateTime fechaFinal, string inventario, string servidor)
        {
            DBResponse<List<Configuraciones>> response = new DBResponse<List<Configuraciones>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from a in db.Configuraciones
                                 where a.Activo == true && a.Deshabilitado == false
                                    && (a.FechaRegistro >= fechaInicial && a.FechaRegistro <= fechaFinal)
                                    && (String.IsNullOrEmpty(inventario) || (!String.IsNullOrEmpty(inventario) && a.NombreInventario.Contains(inventario)))
                                    && (String.IsNullOrEmpty(servidor) || (!String.IsNullOrEmpty(servidor) && a.ServidorIP.Contains(servidor)))
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

        public DBResponse<Configuraciones> Search_ConfiguracionByID(int idConfiguracion)
        {
            DBResponse<Configuraciones> response = new DBResponse<Configuraciones>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = db.Configuraciones.Find(idConfiguracion);

                    if (query != null)
                    {
                        response.ExecutionOK = true;
                        response.Data = query;
                    }
                    else
                    {
                        response.Message = "No se encontro información de la Configuración solicitada.";
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

        public DBResponse<Configuraciones> Upsert_Configuracion(Configuraciones configuracion, ConexSAP conexSAP)
        {
            DBResponse<Configuraciones> response = new DBResponse<Configuraciones>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    if (configuracion.IdConfiguracion == 0)
                    {
                        var tran = db.Database.BeginTransaction();

                        //Obtenemos los Articulos con sus Precios
                        List<Report_ItemsByAlmacen_Result> productos = db.Report_ItemsByAlmacen(configuracion.Almacen).ToList();
                        //Obtenemos los codigos de Barra
                        List<Report_SAPBarcodes_Result> codes = db.Report_SAPBarcodes().ToList();

                        if ((productos != null && productos.Count > 0) && (codes != null && codes.Count > 0))
                        {
                            configuracion.FechaRegistro = DateTime.Now;
                            configuracion.Activo = true;
                            configuracion.Procesado = false;
                            configuracion.AjustesAplicados = false;
                            db.Configuraciones.Add(configuracion);
                            db.SaveChanges();

                            response.Data = configuracion;

                            ///////////////////////////////////////////////////////////////////
                            //Creacion de Base de datos////////////////////////////////////////
                            ///////////////////////////////////////////////////////////////////
                            SqlConnection cnn = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(configuracion.ServidorIP, configuracion.Instancia, configuracion.Puerto, "master", configuracion.Usuario, configuracion.Password));

                            string s = Querys_SAP_SQL.Get_SQL_CrearBaseDatos(configuracion.BaseDatos);
                            SqlCommand cmd = new SqlCommand(s, cnn);

                            try
                            {
                                cnn.Open();
                                cmd.ExecuteNonQuery();

                                SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(configuracion.ServidorIP, configuracion.Instancia, configuracion.Puerto, configuracion.BaseDatos, configuracion.Usuario, configuracion.Password));

                                string sTb = Querys_SAP_SQL.Get_SQL_CrearTabla_Zonas();
                                SqlCommand cmdDB = new SqlCommand(sTb, cnnDB);

                                cnnDB.Open();
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CrearTabla_Marbetes();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CrearTabla_MarbetesArticulos();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CrearTabla_Procesado();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CrearTabla_SAPArticulos();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CrearTabla_SAPArticulosBarCode();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CreateSPItemsSAP();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_CreateSPBarcodesSAP();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.ExecuteNonQuery();

                                XDocument xmlItems = new XDocument(
                                    new XElement("Procesar",
                                        productos.Select(p => new XElement("Item",
                                            new XElement("WhsCode", p.WhsCode),
                                            new XElement("ItemCode", p.ItemCode),
                                            new XElement("ItemName", p.ItemName),
                                            new XElement("Price", p.AvgPrice)
                                        ))
                                    )
                                );

                                XDocument xmlCodigos = new XDocument(
                                    new XElement("Procesar",
                                        codes.Select(p => new XElement("Item",
                                            new XElement("ItemCode", p.ItemCode),
                                            new XElement("BcdCode", p.BcdCode),
                                            new XElement("UomCode", p.Uomcode),
                                            new XElement("BaseQty", p.BaseQty)
                                        ))
                                    )
                                );

                                sTb = Querys_SAP_SQL.Get_SQL_ExecuteInsertItemsSAP();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.Parameters.Clear();
                                cmdDB.Parameters.AddWithValue("@XmlStr", xmlItems.ToString());
                                cmdDB.ExecuteNonQuery();

                                sTb = Querys_SAP_SQL.Get_SQL_ExecuteInsertBarcodesSAP();
                                cmdDB = new SqlCommand(sTb, cnnDB);
                                cmdDB.Parameters.Clear();
                                cmdDB.Parameters.AddWithValue("@XmlStr", xmlCodigos.ToString());
                                cmdDB.ExecuteNonQuery();


                                /*
                                //Traspasamos los Productos
                                foreach (var item in productos)
                                {
                                    sTb = Querys_SAP_SQL.Insert_SQL_SAPArticulo();
                                    cmdDB = new SqlCommand(sTb, cnnDB);
                                    cmdDB.Parameters.Clear();
                                    cmdDB.Parameters.AddWithValue("@WhsCode", item.WhsCode);
                                    cmdDB.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                    cmdDB.Parameters.AddWithValue("@ItemName", item.ItemName);
                                    cmdDB.Parameters.AddWithValue("@Price", item.AvgPrice.GetValueOrDefault());
                                    cmdDB.ExecuteNonQuery();
                                }

                                //Traspasamos los Codigos de Barra
                                foreach (var code in codes)
                                {
                                    sTb = Querys_SAP_SQL.Insert_SQL_SAPArticulosBarCodes();
                                    cmdDB = new SqlCommand(sTb, cnnDB);
                                    cmdDB.Parameters.Clear();
                                    cmdDB.Parameters.AddWithValue("@ItemCode", code.ItemCode);
                                    cmdDB.Parameters.AddWithValue("@BcdCode", code.BcdCode);
                                    cmdDB.Parameters.AddWithValue("@UomCode", code.Uomcode);
                                    cmdDB.Parameters.AddWithValue("@BaseQty", code.BaseQty.GetValueOrDefault());
                                    cmdDB.ExecuteNonQuery();
                                }
                                */

                                if (cnnDB.State == ConnectionState.Open)
                                    cnn.Close();

                                response.ExecutionOK = true;
                                tran.Commit();
                            }
                            catch (Exception exConSQL)
                            {
                                tran.Rollback();
                                response.Message += exConSQL.Message;
                            }
                        }
                        else
                        {
                            response.ExecutionOK = false;
                            response.Message = "No se encontraro los Catalogos de los Productos y/o Costos en el SAP, favor de verificarlo";
                        }
                    }
                    else
                    {
                        var entry = db.Configuraciones.Find(configuracion.IdConfiguracion);
                        if (entry != null)
                        {
                            entry.NombreInventario = configuracion.NombreInventario;
                            entry.ServidorIP = configuracion.ServidorIP;
                            entry.Instancia = configuracion.Instancia;
                            entry.Usuario = configuracion.Usuario;
                            entry.Password = configuracion.Password;
                            entry.BaseDatos = configuracion.BaseDatos;
                            entry.Almacen = configuracion.Almacen;
                            db.SaveChanges();

                            response.ExecutionOK = true;
                            response.Data = entry;
                        }
                        else
                        {
                            response.Message = "La Configuración del Servidor no existe";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message += ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<Configuraciones> Delete_Configuracion(int idConfiguracion)
        {
            DBResponse<Configuraciones> response = new DBResponse<Configuraciones>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {

                    var entry = db.Configuraciones.Find(idConfiguracion);
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
                        response.Message = "La Configuración del Servidor no existe";
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

        public DBResponse<Configuraciones> Cerrar_Configuracion(int idConfiguracion)
        {
            var response = new DBResponse<Configuraciones>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                dbConfiguracion.ServidorIP,
                                dbConfiguracion.Instancia,
                                dbConfiguracion.Puerto,
                                dbConfiguracion.BaseDatos,
                                dbConfiguracion.Usuario,
                                dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_Marbetes(), cnnDB);
                        DataTable dtMarbetes = new DataTable();
                        SqlDataAdapter daMarbetes = new SqlDataAdapter(cmdMarbetes);
                        daMarbetes.Fill(dtMarbetes);

                        bool todosCerrados = true;
                        if (dtMarbetes.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtMarbetes.Rows)
                            {
                                int estatus = int.Parse(row[6].ToString());
                                if (estatus == 1)
                                {
                                    todosCerrados = false;
                                    break;
                                }
                            }
                        }

                        if (todosCerrados)
                        {
                            dbConfiguracion.Cerrado = true;
                            dbConfiguracion.FechaCerrado = DateTime.Now;
                            db.SaveChanges();

                            response.ExecutionOK = true;
                            response.Data = dbConfiguracion;
                        }
                        else
                        {
                            response.Message = "No es posible cerrar el Inventario ya que hay Marbetes sin cerrar";
                        }
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<Configuraciones> Deshabilitar_Configuracion(int idConfiguracion)
        {
            var response = new DBResponse<Configuraciones>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        dbConfiguracion.Deshabilitado = true;
                        dbConfiguracion.FechaDeshabilitado = DateTime.Now;
                        db.SaveChanges();

                        response.ExecutionOK = true;
                        response.Data = dbConfiguracion;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<int> Generate_BackupInventario(int idConfiguracion, string directorioBackups)
        {
            var response = new DBResponse<int>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        directorioBackups = "C:\\Backups\\";

                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                        dbConfiguracion.ServidorIP,
                                        dbConfiguracion.Instancia,
                                        dbConfiguracion.Puerto,
                                        dbConfiguracion.BaseDatos,
                                        dbConfiguracion.Usuario,
                                        dbConfiguracion.Password));

                        // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
                        var backupFileName = String.Format("{0}{1}_{2}.bak",
                            directorioBackups, dbConfiguracion.BaseDatos,
                            DateTime.Now.ToString("yyyyMMdd"));

                        const string query = @"
                            BACKUP DATABASE @db
                            TO DISK = @file;
                            ";

                        using (var command = new SqlCommand(query, cnnDB))
                        {
                            command.Parameters.Add("@db", SqlDbType.NVarChar, 128).Value = dbConfiguracion.BaseDatos;
                            command.Parameters.Add("@file", SqlDbType.NVarChar, 255).Value = backupFileName;

                            cnnDB.Open();
                            command.ExecuteNonQuery();
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();

                        response.ExecutionOK = true;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<List<Zonas>> Get_Zonas(int idConfiguracion, DateTime fechaInicial, DateTime fechaFinal, string zona)
        {
            DBResponse<List<Zonas>> response = new DBResponse<List<Zonas>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from a in db.Zonas
                                 where a.Activo == true
                                    && a.IdConfiguracion == idConfiguracion
                                    && (a.FechaRegistro >= fechaInicial && a.FechaRegistro <= fechaFinal)
                                    && (String.IsNullOrEmpty(zona) || (!String.IsNullOrEmpty(zona) && a.Zona.Contains(zona)))
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

        public DBResponse<Zonas> Search_ZonaByID(int idZona)
        {
            DBResponse<Zonas> response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = db.Zonas.Find(idZona);

                    if (query != null)
                    {
                        response.ExecutionOK = true;
                        response.Data = query;
                    }
                    else
                    {
                        response.Message = "No se encontro información de la Zona solicitada.";
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

        public DBResponse<Zonas> Search_ZonaMarbeteInicial(int idConfiguracion)
        {
            DBResponse<Zonas> response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from i in db.Zonas
                                 where i.IdConfiguracion == idConfiguracion
                                    && i.Activo == true
                                 select i).ToList();

                    if (query.Count > 0)
                    {
                        int maxMarbete = query.Max(m => m.MarbeteFinal);
                        Zonas zona = query.Where(w => w.MarbeteFinal == maxMarbete).FirstOrDefault();

                        response.ExecutionOK = true;
                        response.Data = zona;
                    }
                    else
                    {
                        response.ExecutionOK = true;
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

        public DBResponse<Zonas> Insert_Zona(Zonas zona)
        {
            DBResponse<Zonas> response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {

                    var query = (from i in db.Zonas
                                 where i.IdConfiguracion == zona.IdConfiguracion
                                    && i.Zona == zona.Zona
                                    && i.Activo == true
                                 select i).FirstOrDefault();

                    if (query != null)
                    {
                        response.Message = "El nombre de la Zona ya existe, ingrese otro";
                        response.ExecutionOK = false;
                    }
                    else
                    {
                        var tran = db.Database.BeginTransaction();
                        bool exOk = false;

                        zona.MarbetesCreados = false;
                        zona.FechaRegistro = DateTime.Now;
                        zona.Activo = true;

                        db.Zonas.Add(zona);
                        db.SaveChanges();

                        response.ExecutionOK = true;
                        response.Data = zona;

                        ///////////////////////////////////////////////////////////////////
                        //Registramos la zona en el servidor de la configuración///////////
                        ///////////////////////////////////////////////////////////////////
                        var configuracion = db.Configuraciones.Find(zona.IdConfiguracion);
                        if (configuracion != null)
                        {
                            try
                            {
                                SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(configuracion.ServidorIP, configuracion.Instancia, configuracion.Puerto, configuracion.BaseDatos, configuracion.Usuario, configuracion.Password));

                                string sTb = Querys_SAP_SQL.Insert_SQL_Zona();
                                SqlCommand cmdDB = new SqlCommand(sTb, cnnDB);
                                cnnDB.Open();
                                cmdDB.Parameters.Clear();
                                cmdDB.Parameters.AddWithValue("@IdZona", zona.IdZona);
                                cmdDB.Parameters.AddWithValue("@Zona", zona.Zona);
                                cmdDB.Parameters.AddWithValue("@MarbeteInicial", zona.MarbeteInicial);
                                cmdDB.Parameters.AddWithValue("@MarbeteFinal", zona.MarbeteFinal);

                                cmdDB.ExecuteNonQuery();

                                exOk = true;
                            }
                            catch (Exception exConSQL)
                            {
                                response.Message = exConSQL.Message;
                                response.ExecutionOK = false;
                            }
                        }
                        else
                        {
                            response.ExecutionOK = false;
                            response.Message = "No es posible registrar la Zona en el servidor de la configuración";
                        }
                        ///////////////////////////////////////////////////////////////////
                        ///////////////////////////////////////////////////////////////////

                        if (exOk)
                            tran.Commit();
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

        public DBResponse<Zonas> Delete_Zona(int idZona)
        {
            DBResponse<Zonas> response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {

                    var entry = db.Zonas.Find(idZona);
                    if (entry != null)
                    {
                        var dbConfiguracion = db.Configuraciones.Find(entry.IdConfiguracion);
                        if (dbConfiguracion != null)
                        {
                            if (dbConfiguracion.Cerrado == false)
                            {

                                int queryIdMaxZona = (from i in db.Zonas
                                                      where i.Activo == true
                                                        && i.IdConfiguracion == dbConfiguracion.IdConfiguracion
                                                      select i).Max(m => m.IdZona);

                                if (queryIdMaxZona == idZona)
                                {
                                    SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                        dbConfiguracion.ServidorIP,
                                        dbConfiguracion.Instancia,
                                        dbConfiguracion.Puerto,
                                        dbConfiguracion.BaseDatos,
                                        dbConfiguracion.Usuario,
                                        dbConfiguracion.Password));

                                    cnnDB.Open();

                                    SqlCommand cmdZonaMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbetesByIdZona(), cnnDB);
                                    cmdZonaMarbetes.Parameters.AddWithValue("@IdZona", idZona);

                                    DataTable dtZonas = new DataTable();
                                    SqlDataAdapter daZonas = new SqlDataAdapter(cmdZonaMarbetes);
                                    daZonas.Fill(dtZonas);

                                    if (dtZonas.Rows.Count > 0)
                                    {
                                        response.Message = "No es posible eliminar la Zona ya que hay Marbetes Creados";
                                    }
                                    else
                                    {
                                        SqlCommand cmdDeleteZona = new SqlCommand(Querys_SAP_SQL.Delete_SQL_Zona(), cnnDB);
                                        cmdDeleteZona.Parameters.AddWithValue("@IdZona", idZona);
                                        cmdDeleteZona.ExecuteNonQuery();

                                        if (cnnDB.State == ConnectionState.Open)
                                            cnnDB.Close();

                                        entry.Activo = false;
                                        entry.FechaEliminado = DateTime.Now;
                                        db.SaveChanges();

                                        response.ExecutionOK = true;
                                        response.Data = entry;
                                    }
                                }
                                else
                                {
                                    response.Message = "No es posible eliminar la Zona ya que hay otras creadas posteriormente a la seleccionada";
                                }
                            }
                            else
                            {
                                response.Message = "No se puede eliminar la Zona debido a que el inventario se encuentra cerrado";
                            }
                        }
                        else
                        {
                            response.Message = "No es posible hacer el movimiento";
                        }
                    }
                    else
                    {
                        response.Message = "La Zona no existe";
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

        public DBResponse<List<Codigos>> Get_CodigosByConfiguracion(int idConfiguracion)
        {
            DBResponse<List<Codigos>> response = new DBResponse<List<Codigos>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from a in db.Codigos.Include("Usuarios").Include("Usuarios1").Include("AccionesCodigo").Include("EstatusCodigo")
                                 where a.IdConfiguracion == idConfiguracion
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

        public DBResponse<Codigos> Insert_Codigo(Codigos codigo)
        {
            DBResponse<Codigos> response = new DBResponse<Codigos>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    db.Codigos.Add(codigo);
                    db.SaveChanges();

                    response.ExecutionOK = true;
                    response.Data = codigo;
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

        public DBResponse<Zonas> Create_ZonaMarbetes(int idZona)
        {
            DBResponse<Zonas> response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var zona = db.Zonas.Find(idZona);
                    if (zona != null)
                    {
                        ///////////////////////////////////////////////////////////////////
                        //Revisamos y registramos los Marbetes en el servidor//////////////
                        ///////////////////////////////////////////////////////////////////
                        var configuracion = db.Configuraciones.Find(zona.IdConfiguracion);
                        if (configuracion != null)
                        {
                            SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(configuracion.ServidorIP, configuracion.Instancia, configuracion.Puerto, configuracion.BaseDatos, configuracion.Usuario, configuracion.Password));

                            cnnDB.Open();

                            SqlTransaction transaction = cnnDB.BeginTransaction();
                            SqlCommand cmdDB = cnnDB.CreateCommand();
                            cmdDB.Connection = cnnDB;
                            cmdDB.Transaction = transaction;

                            bool exOk = true;

                            try
                            {
                                for (int marbete = zona.MarbeteInicial; marbete <= zona.MarbeteFinal; marbete++)
                                {
                                    cmdDB.CommandText = "SELECT IdZona FROM Marbetes WHERE IdZona = " + zona.IdZona + " AND Marbete = " + marbete;
                                    object result = cmdDB.ExecuteScalar();
                                    int totalRegistros = Convert.ToInt32(result);

                                    if (totalRegistros == 0)
                                    {
                                        cmdDB.CommandText = "INSERT INTO Marbetes (IdZona, Marbete, Estatus) VALUES(" + zona.IdZona + ", " + marbete + ", 1)";
                                        cmdDB.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        exOk = false;
                                        response.Message = "No fue posible crear los Marbetes debido a que ya existen.";
                                        break;
                                    }
                                }

                                if (exOk)
                                {
                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                            catch (Exception exConSQL)
                            {
                                response.Message = exConSQL.Message;
                                response.ExecutionOK = false;
                                exOk = false;
                            }
                            finally
                            {
                                if (cnnDB.State == ConnectionState.Open)
                                    cnnDB.Close();
                            }


                            if (exOk)
                            {
                                zona.MarbetesCreados = true;
                                db.SaveChanges();

                                response.Message = "Los Marbetes se crearon correctamente";
                                response.ExecutionOK = true;
                            }
                        }
                        else
                        {
                            response.ExecutionOK = false;
                            response.Message = "No es posible registrar la Zona en el servidor de la configuración";
                        }
                    }
                    else
                    {
                        response.ExecutionOK = false;
                        response.Message = "No se encontro información de la Zona";
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

        public DBResponse<Zonas> Delete_ZonaMarbetes(int idZona)
        {
            var response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var zona = db.Zonas.Find(idZona);
                    if (zona != null)
                    {
                        var dbConfiguracion = db.Configuraciones.Find(zona.IdConfiguracion);
                        if (dbConfiguracion != null)
                        {
                            if (dbConfiguracion.Activo == true && dbConfiguracion.Cerrado == false && dbConfiguracion.Deshabilitado == false && dbConfiguracion.AjustesAplicados == false)
                            {
                                SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                dbConfiguracion.ServidorIP,
                                dbConfiguracion.Instancia,
                                dbConfiguracion.Puerto,
                                dbConfiguracion.BaseDatos,
                                dbConfiguracion.Usuario,
                                dbConfiguracion.Password));

                                cnnDB.Open();

                                SqlCommand cmdInfoMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_InfoMarbetesByIdZona(), cnnDB);
                                cmdInfoMarbetes.Parameters.AddWithValue("@IdZona", idZona);

                                DataTable dtInfo = new DataTable();
                                SqlDataAdapter daInfo = new SqlDataAdapter(cmdInfoMarbetes);
                                daInfo.Fill(dtInfo);

                                if (dtInfo.Rows.Count == 0)
                                {
                                    SqlCommand cmdMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbetesByIdZona(), cnnDB);
                                    cmdMarbetes.Parameters.AddWithValue("@IdZona", idZona);

                                    DataTable dtMarbetes = new DataTable();
                                    SqlDataAdapter daMarbetes = new SqlDataAdapter(cmdMarbetes);
                                    daMarbetes.Fill(dtMarbetes);

                                    if (dtMarbetes.Rows.Count > 0)
                                    {
                                        SqlTransaction transaction = cnnDB.BeginTransaction();
                                        SqlCommand cmdDB = new SqlCommand(Querys_SAP_SQL.Delete_SQL_Marbete(), cnnDB);
                                        cmdDB.Transaction = transaction;

                                        foreach (DataRow row in dtMarbetes.Rows)
                                        {
                                            cmdDB.Parameters.Clear();
                                            cmdDB.Parameters.AddWithValue("@IdMarbete", int.Parse(row[0].ToString()));
                                            cmdDB.ExecuteNonQuery();
                                        }

                                        transaction.Commit();
                                        response.ExecutionOK = true;
                                    }
                                    else
                                    {
                                        response.ExecutionOK = true;
                                    }
                                }
                                else
                                {
                                    response.ExecutionOK = false;
                                    response.Message = "No es posible Eliminar los Marbetes ya que actualmente algunos tienen información";
                                }

                                if (cnnDB.State == ConnectionState.Open)
                                    cnnDB.Close();
                            }
                            else
                            {
                                response.Message = "No es posible aplicar la accion debido a que el estatus del inventario lo impide";
                            }
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

        public DBResponse<int> Update_UsoCodigo(UseCodigo uso)
        {
            var response = new DBResponse<int>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(uso.IdConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        var queryCodigo = (from i in db.Codigos
                                           where i.Codigo == uso.Codigo
                                            && i.IdConfiguracion == uso.IdConfiguracion
                                           select i).FirstOrDefault();

                        if (queryCodigo != null)
                        {
                            var dbCodigo = db.Codigos.Find(queryCodigo.IdCodigo);
                            if (dbCodigo.IdEstatus == 1)
                            {
                                dbCodigo.IdUsuarioUtilizo = uso.IdUsuario;
                                dbCodigo.IdAccion = uso.IdAccion;
                                dbCodigo.FechaUtilizo = DateTime.Now;
                                dbCodigo.IdEstatus = 2;
                                db.SaveChanges();

                                response.Data = dbCodigo.IdCodigo;
                                response.ExecutionOK = true;
                            }
                            else
                            {
                                response.Message = "El Código que ingreso ya fue utilizado, ingrese otro.";
                            }
                        }
                        else
                        {
                            response.Message = "El Código que ingreso no es valido.";
                        }
                    }
                    else
                    {
                        response.Message = "No es posible validar el Código que ingreso.";
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

        public DBResponse<Marbetes> Update_MarbeteDescargado(MarbeteDescargado marbete)
        {
            var response = new DBResponse<Marbetes>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(marbete.IdConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        var query = (from m in db.Marbetes
                                     where m.Marbete == marbete.Marbete
                                        && m.Zonas.IdConfiguracion == marbete.IdConfiguracion
                                     select m).FirstOrDefault();
                        if (query != null)
                        {
                            var dbMarbete = db.Marbetes.Find(query.IdMarbete);
                            dbMarbete.Capturo = marbete.NombrePDA;
                            dbMarbete.FechaInicio = marbete.FechaInicio;
                            dbMarbete.FechaCierre = DateTime.Now;
                            dbMarbete.Estatus = 3;
                            db.SaveChanges();

                            response.ExecutionOK = true;
                            response.Data = dbMarbete;
                        }
                        else
                        {
                            response.Message = "No se encontro el Marbete indicado";
                        }
                    }
                    else
                    {
                        response.Message = "No es posible conectarse a la BD";
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

        public DBResponse<List<Listado_AvanceZona>> Get_MonitorAvanceZona(int idConfiguracion)
        {
            var response = new DBResponse<List<Listado_AvanceZona>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var listado = new List<Listado_AvanceZona>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        string almacen = dbConfiguracion.Almacen;
                        var queryZonas = (from i in db.Zonas
                                          where i.Activo == true
                                            && i.IdConfiguracion == idConfiguracion
                                          select i).ToList();
                        if (queryZonas != null && queryZonas.Count > 0)
                        {
                            SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                dbConfiguracion.ServidorIP,
                                dbConfiguracion.Instancia,
                                dbConfiguracion.Puerto,
                                dbConfiguracion.BaseDatos,
                                dbConfiguracion.Usuario,
                                dbConfiguracion.Password));

                            cnnDB.Open();

                            foreach (var itemZona in queryZonas)
                            {
                                decimal avance = 0m;
                                int minutos = 0;
                                string tiempo = "00:00";
                                int cerrados = 0;
                                int totalMarbetes = 0;
                                int pendientes = 0;
                                decimal monto = 0m;
                                string estatus = "Abierto";
                                bool estaCerrado = true;

                                SqlCommand cmdZonaMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbetesByIdZona(), cnnDB);
                                cmdZonaMarbetes.Parameters.AddWithValue("@IdZona", itemZona.IdZona);

                                DataTable dtZonas = new DataTable();
                                SqlDataAdapter daZonas = new SqlDataAdapter(cmdZonaMarbetes);
                                daZonas.Fill(dtZonas);

                                if (dtZonas.Rows.Count > 0)
                                {
                                    foreach (DataRow rowZona in dtZonas.Rows)
                                    {
                                        totalMarbetes++;

                                        int idMarbete = int.Parse(rowZona[0].ToString());
                                        string fechaInicio = rowZona[4].ToString();
                                        DateTime inicio = DateTime.Now;

                                        if (!String.IsNullOrEmpty(fechaInicio))
                                            inicio = DateTime.Parse(fechaInicio);

                                        int estatusMarbete = int.Parse(rowZona[6].ToString());
                                        if (estatusMarbete > 1)
                                        {
                                            cerrados++;
                                        }
                                        else
                                        {
                                            estaCerrado = false;
                                            pendientes++;
                                        }

                                        SqlCommand cmdMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteUltimaCaptura(), cnnDB);
                                        cmdMarbetes.Parameters.AddWithValue("@IdMarbete", idMarbete);

                                        DataTable dtMarbetes = new DataTable();
                                        SqlDataAdapter daMarbetes = new SqlDataAdapter(cmdMarbetes);
                                        daMarbetes.Fill(dtMarbetes);
                                        if (dtMarbetes.Rows.Count > 0)
                                        {
                                            string fechaUltima = dtMarbetes.Rows[0][0].ToString();
                                            DateTime fin = DateTime.Now;

                                            if (!String.IsNullOrEmpty(fechaUltima))
                                            {
                                                fin = DateTime.Parse(fechaUltima);

                                                TimeSpan diferencia = fin - inicio;
                                                minutos += diferencia.Minutes;
                                            }
                                        }

                                        SqlCommand cmdMarbetesArticulos = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteArticulos(), cnnDB);
                                        cmdMarbetesArticulos.Parameters.AddWithValue("@IdMarbete", idMarbete);

                                        DataTable dtArticulos = new DataTable();
                                        SqlDataAdapter daArticulos = new SqlDataAdapter(cmdMarbetesArticulos);
                                        daArticulos.Fill(dtArticulos);
                                        if (dtArticulos.Rows.Count > 0)
                                        {
                                            foreach (DataRow rowArticulo in dtArticulos.Rows)
                                            {
                                                monto += decimal.Parse(rowArticulo[8].ToString());
                                            }
                                        }
                                    }
                                }

                                if (minutos > 0)
                                {
                                    int nuevosMinutos = minutos;
                                    int horas = minutos / 60;
                                    if (horas >= 1)
                                    {
                                        nuevosMinutos = minutos - (horas * 60);
                                    }

                                    tiempo = horas.ToString().PadLeft(2, '0') + ":" + nuevosMinutos.ToString().PadLeft(2, '0');
                                }

                                if (estaCerrado)
                                {
                                    estatus = "Cerrado";
                                }

                                avance = (cerrados * 100) / totalMarbetes;

                                listado.Add(new Listado_AvanceZona
                                {
                                    IdZona = itemZona.IdZona,
                                    Almacen = almacen,
                                    Zona = itemZona.Zona,
                                    Avance = avance,
                                    Minutos = minutos,
                                    Tiempo = tiempo,
                                    Cerrados = cerrados,
                                    TotalMarbetes = totalMarbetes,
                                    MarbetesPorCapturar = pendientes,
                                    Monto = monto,
                                    Estatus = estatus
                                });
                            }
                        }
                    }

                    response.ExecutionOK = true;
                    response.Data = listado;
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

        public DBResponse<List<Listado_AvanceMarbete>> Get_MonitorAvanceMarbete(int idConfiguracion, int idZona, int marbete)
        {
            var response = new DBResponse<List<Listado_AvanceMarbete>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var listado = new List<Listado_AvanceMarbete>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        string almacen = dbConfiguracion.Almacen;
                        var queryZonas = (from i in db.Zonas
                                          where i.Activo == true
                                            && i.IdConfiguracion == idConfiguracion
                                            && (idZona == 0 || (idZona > 0 && i.IdZona == idZona))
                                          select i).ToList();
                        if (queryZonas != null && queryZonas.Count > 0)
                        {

                            SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                            cnnDB.Open();

                            foreach (Zonas dbZona in queryZonas)
                            {
                                SqlCommand cmdMarbetes = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbetesByIdZonaMarbete(), cnnDB);
                                cmdMarbetes.Parameters.AddWithValue("@IdZona", dbZona.IdZona);
                                cmdMarbetes.Parameters.AddWithValue("@Marbete", marbete);

                                DataTable dtMarbetes = new DataTable();
                                SqlDataAdapter daMarbetes = new SqlDataAdapter(cmdMarbetes);
                                daMarbetes.Fill(dtMarbetes);

                                if (dtMarbetes.Rows.Count > 0)
                                {
                                    foreach (DataRow rowMarbete in dtMarbetes.Rows)
                                    {
                                        int idMarbete = int.Parse(rowMarbete[0].ToString());
                                        int idEstatus = int.Parse(rowMarbete[6].ToString());
                                        string estatus = idEstatus == 1 ? "Abierto" : "Cerrado";
                                        decimal importe = 0m;
                                        int minutos = 0;
                                        string tiempo = "00:00";
                                        string fechaInicio = rowMarbete[4].ToString();
                                        DateTime inicio = DateTime.Now;
                                        if (!String.IsNullOrEmpty(fechaInicio))
                                            inicio = DateTime.Parse(fechaInicio);

                                        SqlCommand cmdMarbetesUltimo = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteUltimaCaptura(), cnnDB);
                                        cmdMarbetesUltimo.Parameters.AddWithValue("@IdMarbete", idMarbete);

                                        DataTable dtMarbetesUltimo = new DataTable();
                                        SqlDataAdapter daMarbetesUltimo = new SqlDataAdapter(cmdMarbetesUltimo);
                                        daMarbetesUltimo.Fill(dtMarbetesUltimo);
                                        if (dtMarbetesUltimo.Rows.Count > 0)
                                        {
                                            string fechaUltima = dtMarbetesUltimo.Rows[0][0].ToString();
                                            DateTime fin = DateTime.Now;
                                            if (!String.IsNullOrEmpty(fechaUltima))
                                            {
                                                fin = DateTime.Parse(fechaUltima);

                                                TimeSpan diferencia = fin - inicio;
                                                minutos = diferencia.Minutes;
                                            }
                                        }

                                        SqlCommand cmdMarbetesArticulos = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteArticulos(), cnnDB);
                                        cmdMarbetesArticulos.Parameters.AddWithValue("@IdMarbete", idMarbete);

                                        DataTable dtArticulos = new DataTable();
                                        SqlDataAdapter daArticulos = new SqlDataAdapter(cmdMarbetesArticulos);
                                        daArticulos.Fill(dtArticulos);
                                        if (dtArticulos.Rows.Count > 0)
                                        {
                                            foreach (DataRow rowArticulo in dtArticulos.Rows)
                                            {
                                                importe += decimal.Parse(rowArticulo[8].ToString());
                                            }
                                        }

                                        if (minutos > 0)
                                        {
                                            int nuevosMinutos = minutos;
                                            int horas = minutos / 60;
                                            if (horas >= 1)
                                            {
                                                nuevosMinutos = minutos - (horas * 60);
                                            }

                                            tiempo = horas.ToString().PadLeft(2, '0') + ":" + nuevosMinutos.ToString().PadLeft(2, '0');
                                        }

                                        listado.Add(new Listado_AvanceMarbete
                                        {
                                            Almacen = dbConfiguracion.Almacen,
                                            Zona = dbZona.Zona,
                                            IdMarbete = idMarbete,
                                            Marbete = int.Parse(rowMarbete[2].ToString()),
                                            IdEstatus = idEstatus,
                                            Estatus = estatus,
                                            Importe = importe,
                                            TotalMinutos = minutos,
                                            Tiempo = tiempo,
                                            Nombre = rowMarbete[3].ToString()
                                        });
                                    }
                                }
                            }

                            if (cnnDB.State == ConnectionState.Open)
                                cnnDB.Close();

                            response.ExecutionOK = true;
                            response.Data = listado;
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

        public DBResponse<List<Listado_AvanceDetalleMarbete>> Get_MonitorAvanceDetalleMarbete(int idConfiguracion, int marbete)
        {
            var response = new DBResponse<List<Listado_AvanceDetalleMarbete>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var listado = new List<Listado_AvanceDetalleMarbete>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        string almacen = dbConfiguracion.Almacen;

                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdMarbete = new SqlCommand(Querys_SAP_SQL.Get_SQL_IdMarbeteByMarbete(), cnnDB);
                        cmdMarbete.Parameters.AddWithValue("@Marbete", marbete);

                        DataTable dtMarbete = new DataTable();
                        SqlDataAdapter daMarbete = new SqlDataAdapter(cmdMarbete);
                        daMarbete.Fill(dtMarbete);

                        if (dtMarbete.Rows.Count > 0)
                        {
                            int idMarbete = int.Parse(dtMarbete.Rows[0][0].ToString());

                            SqlCommand cmdMarbetesArticulos = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteArticulos(), cnnDB);
                            cmdMarbetesArticulos.Parameters.AddWithValue("@IdMarbete", idMarbete);

                            DataTable dtArticulos = new DataTable();
                            SqlDataAdapter daArticulos = new SqlDataAdapter(cmdMarbetesArticulos);
                            daArticulos.Fill(dtArticulos);
                            if (dtArticulos.Rows.Count > 0)
                            {
                                foreach (DataRow rowArticulo in dtArticulos.Rows)
                                {
                                    DateTime fechaCaptura = DateTime.Parse(rowArticulo[11].ToString());

                                    listado.Add(new Listado_AvanceDetalleMarbete
                                    {
                                        IdConfiguracion = idConfiguracion,
                                        Folio = int.Parse(rowArticulo[0].ToString()),
                                        Almacen = almacen,
                                        Marbete = marbete,
                                        CodigoBarras = rowArticulo[3].ToString(),
                                        ItemCode = rowArticulo[2].ToString(),
                                        ItemName = rowArticulo[4].ToString(),
                                        Cantidad = decimal.Parse(rowArticulo[9].ToString()),
                                        UOM = rowArticulo[5].ToString(),
                                        TimeCount1 = fechaCaptura,
                                        Nombre = rowArticulo[10].ToString()
                                    });
                                }
                            }
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();
                    }

                    response.ExecutionOK = true;
                    response.Data = listado;
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

        public DBResponse<int> Delete_ArticuloMarbete(int idConfiguracion, int idDetalle)
        {
            var response = new DBResponse<int>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        string almacen = dbConfiguracion.Almacen;

                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdDeleteArticulo = new SqlCommand(Querys_SAP_SQL.Delete_SQL_MarbeteDetalle(), cnnDB);
                        cmdDeleteArticulo.Parameters.AddWithValue("@IdDetalle", idDetalle);
                        cmdDeleteArticulo.ExecuteNonQuery();

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();
                    }

                    response.ExecutionOK = true;
                    response.Data = idDetalle;
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

        public DBResponse<DatosBarcode> Get_DatosProducto(int idConfiguracion, string codigo)
        {
            var response = new DBResponse<DatosBarcode>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    DatosBarcode datos = new DatosBarcode();
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        string almacen = dbConfiguracion.Almacen;

                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdCodigo = new SqlCommand(Querys_SAP_SQL.Get_InfoBarcode(), cnnDB);
                        cmdCodigo.Parameters.AddWithValue("@BcdCode", codigo);

                        DataTable dtCodigo = new DataTable();
                        SqlDataAdapter daCodigo = new SqlDataAdapter(cmdCodigo);
                        daCodigo.Fill(dtCodigo);
                        if (dtCodigo.Rows.Count > 0)
                        {
                            datos.ItemCode = dtCodigo.Rows[0][0].ToString();
                            datos.Uom = dtCodigo.Rows[0][2].ToString();

                            SqlCommand cmdProducto = new SqlCommand(Querys_SAP_SQL.Get_InfoProducto(), cnnDB);
                            cmdProducto.Parameters.AddWithValue("@ItemCode", datos.ItemCode);

                            DataTable dtProducto = new DataTable();
                            SqlDataAdapter daProducto = new SqlDataAdapter(cmdProducto);
                            daProducto.Fill(dtProducto);
                            if (dtProducto.Rows.Count > 0)
                            {
                                datos.Descripcion = dtProducto.Rows[0][2].ToString();

                                response.ExecutionOK = true;
                                response.Data = datos;
                            }
                            else
                            {
                                response.Message = "No se encontro información del Producto";
                            }
                        }
                        else
                        {
                            response.Message = "No se encontro datos del Código ingresado";
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();
                    }
                    else
                    {
                        response.Message = "No se encontro información para conectarse al inventario seleccionado";
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

        public DBResponse<int> Insert_Producto(int idConfiguracion, int marbete, string codigo, decimal cantidad, string usuario)
        {
            var response = new DBResponse<int>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion == null || dbConfiguracion.Cerrado || dbConfiguracion.Procesado)
                    {
                        response.Message = "El Inventario no está disponible para captura";
                        return response;
                    }

                    using (SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                        dbConfiguracion.ServidorIP, dbConfiguracion.Instancia, dbConfiguracion.Puerto,
                        dbConfiguracion.BaseDatos, dbConfiguracion.Usuario, dbConfiguracion.Password)))
                    {
                        cnnDB.Open();

                        // CONSULTA UNIFICADA con JOIN
                        string sqlProducto = @"
                    SELECT 
                        B.ItemCode, B.UomCode, B.BaseQty,
                        A.ItemName, A.Price
                    FROM SAP_Barcodes B
                    LEFT JOIN SAP_Articulos A ON B.ItemCode = A.ItemCode
                    WHERE B.BcdCode = @BcdCode";

                        DataTable dtProducto = new DataTable();
                        using (SqlCommand cmd = new SqlCommand(sqlProducto, cnnDB))
                        {
                            cmd.Parameters.AddWithValue("@BcdCode", codigo);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dtProducto);
                            }
                        }

                        if (dtProducto.Rows.Count == 0)
                        {
                            response.Message = "No se encontró información del código ingresado";
                            return response;
                        }

                        DataRow row = dtProducto.Rows[0];
                        string itemCode = row["ItemCode"].ToString();
                        string uom = row["UomCode"].ToString();
                        decimal baseQty = Convert.ToDecimal(row["BaseQty"]);
                        string descripcion = row["ItemName"].ToString();
                        decimal precio = Convert.ToDecimal(row["Price"]);

                        // Obtener IdMarbete
                        int idMarbete = 0;
                        using (SqlCommand cmd = new SqlCommand(Querys_SAP_SQL.Get_SQL_IdMarbeteByMarbete(), cnnDB))
                        {
                            cmd.Parameters.AddWithValue("@Marbete", marbete);
                            object result = cmd.ExecuteScalar();
                            if (result != null) idMarbete = Convert.ToInt32(result);
                        }

                        if (idMarbete == 0)
                        {
                            response.Message = "No se encontró el marbete especificado";
                            return response;
                        }

                        decimal costo = (cantidad * baseQty) * precio;

                        // INSERTAR en una sola consulta
                        string sqlInsert = @"
                    INSERT INTO MarbetesArticulos 
                        (IdMarbete, CodigoArticulo, CodigoBarras, Descripcion, Grupo, 
                         Uom, BaseQty, Precio, Costo, Cantidad, Cantidad2, Capturo, FechaCaptura)
                    VALUES 
                        (@IdMarbete, @CodigoArticulo, @CodigoBarras, @Descripcion, @Grupo,
                         @Uom, @BaseQty, @Precio, @Costo, @Cantidad, @Cantidad2, @Capturo, @FechaCaptura)";

                        using (SqlCommand cmd = new SqlCommand(sqlInsert, cnnDB))
                        {
                            cmd.Parameters.AddWithValue("@IdMarbete", idMarbete);
                            cmd.Parameters.AddWithValue("@CodigoArticulo", itemCode);
                            cmd.Parameters.AddWithValue("@CodigoBarras", codigo);
                            cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                            cmd.Parameters.AddWithValue("@Grupo", "");
                            cmd.Parameters.AddWithValue("@Uom", uom);
                            cmd.Parameters.AddWithValue("@BaseQty", baseQty);
                            cmd.Parameters.AddWithValue("@Precio", precio);
                            cmd.Parameters.AddWithValue("@Costo", costo);
                            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                            cmd.Parameters.AddWithValue("@Cantidad2", cantidad);
                            cmd.Parameters.AddWithValue("@Capturo", usuario);
                            cmd.Parameters.AddWithValue("@FechaCaptura", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }

                        response.ExecutionOK = true;
                        response.Data = 0;
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

        public DBResponse<int> Update_CantidadProducto(int idConfiguracion, int idDetalle, decimal cantidad)
        {
            var response = new DBResponse<int>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        if (dbConfiguracion.Activo == true && dbConfiguracion.Deshabilitado == false && dbConfiguracion.Cerrado == false && dbConfiguracion.Procesado == false)
                        {
                            SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                            cnnDB.Open();

                            decimal costo = 0m;

                            SqlCommand cmdDetalle = new SqlCommand(Querys_SAP_SQL.Get_InfoProductoMarbete(), cnnDB);
                            cmdDetalle.Parameters.AddWithValue("@IdDetalle", idDetalle);

                            DataTable dtDetalle = new DataTable();
                            SqlDataAdapter daDetalle = new SqlDataAdapter(cmdDetalle);
                            daDetalle.Fill(dtDetalle);
                            if (dtDetalle.Rows.Count > 0)
                            {
                                costo = decimal.Parse(dtDetalle.Rows[0][0].ToString()) * cantidad;
                            }

                            SqlCommand cmdUpdateArticulo = new SqlCommand(Querys_SAP_SQL.Update_CantidadProducto(), cnnDB);
                            cmdUpdateArticulo.Parameters.AddWithValue("@Cantidad", cantidad);
                            cmdUpdateArticulo.Parameters.AddWithValue("@Costo", costo);
                            cmdUpdateArticulo.Parameters.AddWithValue("@IdDetalle", idDetalle);
                            cmdUpdateArticulo.ExecuteNonQuery();

                            if (cnnDB.State == ConnectionState.Open)
                                cnnDB.Close();

                            response.ExecutionOK = true;
                            response.Data = idDetalle;
                        }
                        else
                        {
                            response.Message = "No es posible aplicar la modificación debido a que el estatus del Inventario lo impide, reviselo";
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

        public DBResponse<List<DatosBarcode>> Get_MarbeteProductos(int idConfiguracion, int marbete)
        {
            var response = new DBResponse<List<DatosBarcode>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var listado = new List<DatosBarcode>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        int idMarbete = 0;

                        SqlCommand cmdMarbete = new SqlCommand(Querys_SAP_SQL.Get_SQL_IdMarbeteByMarbete(), cnnDB);
                        cmdMarbete.Parameters.AddWithValue("@Marbete", marbete);

                        DataTable dtMarbete = new DataTable();
                        SqlDataAdapter daMarbete = new SqlDataAdapter(cmdMarbete);
                        daMarbete.Fill(dtMarbete);
                        if (dtMarbete.Rows.Count > 0)
                        {
                            idMarbete = int.Parse(dtMarbete.Rows[0][0].ToString());
                        }


                        SqlCommand cmdMarbetesArticulos = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteArticulos(), cnnDB);
                        cmdMarbetesArticulos.Parameters.AddWithValue("@IdMarbete", idMarbete);

                        DataTable dtArticulos = new DataTable();
                        SqlDataAdapter daArticulos = new SqlDataAdapter(cmdMarbetesArticulos);
                        daArticulos.Fill(dtArticulos);
                        if (dtArticulos.Rows.Count > 0)
                        {
                            foreach (DataRow rowArticulo in dtArticulos.Rows)
                            {
                                listado.Add(new DatosBarcode
                                {
                                    Marbete = marbete,
                                    Codigo = rowArticulo[3].ToString(),
                                    ItemCode = rowArticulo[2].ToString(),
                                    Descripcion = rowArticulo[4].ToString(),
                                    Cantidad = Decimal.ToInt32(decimal.Parse(rowArticulo[9].ToString())),
                                    Uom = rowArticulo[5].ToString(),
                                    Precio = decimal.Parse(rowArticulo[7].ToString()),
                                    Costo = decimal.Parse(rowArticulo[8].ToString()),
                                    NombrePDA = rowArticulo[10].ToString()
                                });
                            }
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();

                        response.ExecutionOK = true;
                        response.Data = listado;
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

        public DBResponse<List<Resultado_Capturas>> Get_Procesado(int idConfiguracion, int marbete, int idConteo, string codigoArticulo)
        {
            var response = new DBResponse<List<Resultado_Capturas>>();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    db.Database.CommandTimeout = 1200;
                    var listadoCaptura = new List<Resultado_Capturas>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion == null)
                    {
                        response.Message = "No se encontro información del Inventario";
                        return response;
                    }

                    if (dbConfiguracion.Procesado)
                    {
                        response.Message = "El Inventario ya fue procesado.";
                        return response;
                    }

                    using (SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                        dbConfiguracion.ServidorIP, dbConfiguracion.Instancia, dbConfiguracion.Puerto,
                        dbConfiguracion.BaseDatos, dbConfiguracion.Usuario, dbConfiguracion.Password)))
                    {
                        cnnDB.Open();

                        // Obtener total de marbetes cerrados
                        int marbetesCerrados = 0;
                        using (SqlCommand cmd = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbetesCerrados(), cnnDB))
                        {
                            marbetesCerrados = (int)cmd.ExecuteScalar();
                        }

                        int marbetesTotales = db.Get_TotalMarbetesByConfiguracion(idConfiguracion).FirstOrDefault().GetValueOrDefault();

                        if (marbetesTotales == 0 || marbetesTotales != marbetesCerrados)
                        {
                            response.Message = marbetesTotales == 0
                                ? "No hay Marbetes registrados en el inventario"
                                : "Hay Marbetes abiertos, todos deben estar cerrados para ejecutar este proceso";
                            return response;
                        }

                        // CONSULTA UNIFICADA - Obtiene todos los datos en una sola consulta
                        string sqlUnificado = @"
                    WITH DatosMarbete AS (
                        SELECT 
                            MA.IdDetalle, MA.IdMarbete, M.Marbete, MA.CodigoArticulo, 
                            MA.CodigoBarras, MA.Descripcion, MA.Uom, MA.BaseQty, 
                            MA.Precio, MA.Costo, MA.Cantidad, MA.Cantidad2, MA.Capturo,
                            SA.ItmsGrpNam, SA.OnHand, SA.Price AS PrecioSAP, SA.UomEntry, SA.Uom AS UomSAP
                        FROM MarbetesArticulos MA
                        LEFT JOIN Marbetes M ON MA.IdMarbete = M.IdMarbete
                        LEFT JOIN SAP_Articulos SA ON MA.CodigoArticulo = SA.ItemCode
                    ),
                    Agrupado AS (
                        SELECT 
                            CodigoArticulo,
                            MAX(Descripcion) AS Descripcion,
                            MAX(Uom) AS Uom,
                            MAX(ItmsGrpNam) AS ItmsGrpNam,
                            MAX(PrecioSAP) AS PrecioSAP,
                            MAX(OnHand) AS OnHand,
                            MAX(UomEntry) AS UomEntry,
                            MAX(UomSAP) AS UomSAP,
                            SUM(Cantidad * BaseQty) AS CantidadTotal,
                            STRING_AGG(Marbete, ', ') AS Marbetes
                        FROM DatosMarbete
                        GROUP BY CodigoArticulo
                    )
                    SELECT 
                        CodigoArticulo, Descripcion, Uom, ItmsGrpNam, 
                        PrecioSAP, OnHand, UomEntry, UomSAP,
                        CantidadTotal, Marbetes
                    FROM Agrupado";

                        DataTable dtUnificado = new DataTable();
                        using (SqlCommand cmd = new SqlCommand(sqlUnificado, cnnDB))
                        {
                            cmd.CommandTimeout = 1200;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dtUnificado);
                            }
                        }

                        // PROCESAR DATOS EN MEMORIA
                        List<Resultado_Capturas> resultadoCapturas = new List<Resultado_Capturas>();
                        List<Listado_Procesado> procesadoList = new List<Listado_Procesado>();

                        foreach (DataRow row in dtUnificado.Rows)
                        {
                            string codigoArticulo = row["CodigoArticulo"].ToString();
                            string descripcion = row["Descripcion"].ToString();
                            string uom = row["UomSAP"]?.ToString() ?? row["Uom"]?.ToString() ?? "";
                            string itmsGrpNam = row["ItmsGrpNam"]?.ToString() ?? "";
                            decimal precioSAP = row["PrecioSAP"] != DBNull.Value ? Convert.ToDecimal(row["PrecioSAP"]) : 0;
                            decimal onHand = row["OnHand"] != DBNull.Value ? Convert.ToDecimal(row["OnHand"]) : 0;
                            int uomEntry = row["UomEntry"] != DBNull.Value ? Convert.ToInt32(row["UomEntry"]) : 0;
                            decimal cantidadTotal = row["CantidadTotal"] != DBNull.Value ? Convert.ToDecimal(row["CantidadTotal"]) : 0;
                            string marbetes = row["Marbetes"]?.ToString() ?? "";

                            resultadoCapturas.Add(new Resultado_Capturas
                            {
                                Folio = 0,
                                Marbete = 0,
                                CodigoProducto = codigoArticulo,
                                Descripcion = descripcion,
                                Conteo = idConteo == 1 ? "Conteo 1" : "Conteo 2",
                                Nombre = ""
                            });

                            decimal totalSAP = onHand * precioSAP;
                            decimal totalContado = cantidadTotal * precioSAP;
                            decimal totalDesviacion = totalContado - totalSAP;
                            decimal porcDif = totalSAP == 0 ? 0 : (totalDesviacion / totalSAP) * 100;
                            decimal varianzaNeta = cantidadTotal - onHand;
                            decimal varianzaNetaMonto = Math.Abs(varianzaNeta) * precioSAP;

                            procesadoList.Add(new Listado_Procesado
                            {
                                WhsCode = dbConfiguracion.Almacen,
                                ItmsGrpNam = itmsGrpNam,
                                ItemCode = codigoArticulo,
                                ItemName = descripcion,
                                Cant1 = cantidadTotal,
                                Cant2 = cantidadTotal,
                                TotalSAP = totalSAP,
                                TotalContado = totalContado,
                                TotalDesviacion = totalDesviacion,
                                PorcDif = porcDif,
                                Uom = uom,
                                Onhand = onHand,
                                PorProcesar = 0,
                                DifPesoNeto = varianzaNeta * precioSAP,
                                Marbetes = marbetes,
                                Precio = precioSAP,
                                VarianzaNeta = varianzaNeta,
                                VarianzaNetaMonto = varianzaNetaMonto,
                                TotalContado2 = totalContado,
                                TotalDesviacion2 = totalDesviacion,
                                PorcDif2 = porcDif,
                                IdUom = uomEntry,
                                Contado1 = 0,
                                Contado2 = 0,
                                Contado3 = 0,
                                Contado4 = 0,
                                TotalContadoR = 0,
                                TotalContadoA = cantidadTotal,
                                DiferenciaContado = 0,
                                DiferenciaInventario = varianzaNeta,
                                Modificado1 = 0,
                                Modificado2 = 0,
                                Modificado3 = 0,
                                Modificado4 = 0
                            });
                        }

                        // OBTENER ARTÍCULOS NO CONTADOS
                        var itemCodesProcesados = procesadoList.Select(p => p.ItemCode).ToHashSet();
                        string itemCodesStr = string.Join(",", itemCodesProcesados.Select(c => $"'{c}'"));

                        string sqlNoContados = $@"
                    SELECT WhsCode, ItemCode, ItemName, ItmsGrpNam, OnHand, Price AS Precio, UomEntry, Uom
                    FROM SAP_Articulos 
                    WHERE ItemCode NOT IN ({itemCodesStr})
                      AND WhsCode = @WhsCode";

                        DataTable dtNoContados = new DataTable();
                        using (SqlCommand cmd = new SqlCommand(sqlNoContados, cnnDB))
                        {
                            cmd.Parameters.AddWithValue("@WhsCode", dbConfiguracion.Almacen);
                            cmd.CommandTimeout = 1200;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dtNoContados);
                            }
                        }

                        foreach (DataRow row in dtNoContados.Rows)
                        {
                            string itemCode = row["ItemCode"].ToString();
                            string itemName = row["ItemName"].ToString();
                            string itmsGrpNam = row["ItmsGrpNam"].ToString();
                            decimal onHand = Convert.ToDecimal(row["OnHand"]);
                            decimal precio = Convert.ToDecimal(row["Precio"]);
                            int uomEntry = Convert.ToInt32(row["UomEntry"]);
                            string uom = row["Uom"].ToString();

                            resultadoCapturas.Add(new Resultado_Capturas
                            {
                                Folio = 0,
                                Marbete = 0,
                                CodigoProducto = itemCode,
                                Descripcion = itemName,
                                Conteo = idConteo == 1 ? "Conteo 1" : "Conteo 2",
                                Nombre = ""
                            });

                            procesadoList.Add(new Listado_Procesado
                            {
                                WhsCode = dbConfiguracion.Almacen,
                                ItmsGrpNam = itmsGrpNam,
                                ItemCode = itemCode,
                                ItemName = itemName,
                                Cant1 = 0,
                                Cant2 = 0,
                                TotalSAP = onHand * precio,
                                TotalContado = 0,
                                TotalDesviacion = -(onHand * precio),
                                PorcDif = -100,
                                Uom = uom,
                                Onhand = onHand,
                                PorProcesar = 0,
                                DifPesoNeto = onHand * precio,
                                Marbetes = "0",
                                Precio = precio,
                                VarianzaNeta = -onHand,
                                VarianzaNetaMonto = onHand * precio,
                                TotalContado2 = 0,
                                TotalDesviacion2 = -(onHand * precio),
                                PorcDif2 = -100,
                                IdUom = uomEntry,
                                Contado1 = 0,
                                Contado2 = 0,
                                Contado3 = 0,
                                Contado4 = 0,
                                TotalContadoR = 0,
                                TotalContadoA = 0,
                                DiferenciaContado = 0,
                                DiferenciaInventario = -onHand,
                                Modificado1 = 0,
                                Modificado2 = 0,
                                Modificado3 = 0,
                                Modificado4 = 0
                            });
                        }

                        // GUARDAR EN PROCESADO USANDO SQLBULKCOPY
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Procesado WHERE WhsCode = @WhsCode", cnnDB))
                        {
                            cmd.Parameters.AddWithValue("@WhsCode", dbConfiguracion.Almacen);
                            cmd.ExecuteNonQuery();
                        }

                        DataTable dtProcesado = CrearDataTableProcesado();

                        foreach (var item in procesadoList)
                        {
                            DataRow row = dtProcesado.NewRow();
                            row["WhsCode"] = item.WhsCode ?? "";
                            row["ItmsGrpNam"] = item.ItmsGrpNam ?? "";
                            row["ItemCode"] = item.ItemCode ?? "";
                            row["ItemName"] = item.ItemName ?? "";
                            row["Cant1"] = item.Cant1;
                            row["Cant2"] = item.Cant2;
                            row["Contado1"] = item.Contado1;
                            row["Contado2"] = item.Contado2;
                            row["Contado3"] = item.Contado3;
                            row["Contado4"] = item.Contado4;
                            row["TotalSAP"] = item.TotalSAP;
                            row["TotalContado"] = item.TotalContado;
                            row["TotalDesviacion"] = item.TotalDesviacion;
                            row["PorcDif"] = item.PorcDif;
                            row["Uom"] = item.Uom ?? "";
                            row["Onhand"] = item.Onhand;
                            row["PorProcesar"] = item.PorProcesar;
                            row["DifPesoNeto"] = item.DifPesoNeto;
                            row["Marbetes"] = item.Marbetes ?? "";
                            row["Precio"] = item.Precio;
                            row["VarianzaNeta"] = item.VarianzaNeta;
                            row["VarianzaNetaMonto"] = item.VarianzaNetaMonto;
                            row["TotalContado2"] = item.TotalContado2;
                            row["TotalDesviacion2"] = item.TotalDesviacion2;
                            row["PorcDif2"] = item.PorcDif2;
                            row["Ajustado"] = 0;
                            row["IdUom"] = item.IdUom;
                            row["TotalContadoR"] = item.TotalContadoR;
                            row["TotalContadoA"] = item.TotalContadoA;
                            row["DiferenciaContado"] = item.DiferenciaContado;
                            row["DiferenciaInventario"] = item.DiferenciaInventario;
                            row["Modificado1"] = item.Modificado1;
                            row["Modificado2"] = item.Modificado2;
                            row["Modificado3"] = item.Modificado3;
                            row["Modificado4"] = item.Modificado4;
                            dtProcesado.Rows.Add(row);
                        }

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cnnDB))
                        {
                            bulkCopy.DestinationTableName = "Procesado";
                            bulkCopy.BatchSize = 5000;
                            bulkCopy.BulkCopyTimeout = 1200;
                            bulkCopy.WriteToServer(dtProcesado);
                        }

                        dbConfiguracion.Procesado = true;
                        db.SaveChanges();

                        response.ExecutionOK = true;
                        response.Data = resultadoCapturas;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            watch.Stop();
            return response;
        }

        private DataTable CrearDataTableProcesado()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("WhsCode", typeof(string));
            dt.Columns.Add("ItmsGrpNam", typeof(string));
            dt.Columns.Add("ItemCode", typeof(string));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("Cant1", typeof(decimal));
            dt.Columns.Add("Cant2", typeof(decimal));
            dt.Columns.Add("TotalSAP", typeof(decimal));
            dt.Columns.Add("TotalContado", typeof(decimal));
            dt.Columns.Add("TotalDesviacion", typeof(decimal));
            dt.Columns.Add("PorcDif", typeof(decimal));
            dt.Columns.Add("Uom", typeof(string));
            dt.Columns.Add("Onhand", typeof(decimal));
            dt.Columns.Add("PorProcesar", typeof(decimal));
            dt.Columns.Add("DifPesoNeto", typeof(decimal));
            dt.Columns.Add("Marbetes", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("VarianzaNeta", typeof(decimal));
            dt.Columns.Add("VarianzaNetaMonto", typeof(decimal));
            dt.Columns.Add("TotalContado2", typeof(decimal));
            dt.Columns.Add("TotalDesviacion2", typeof(decimal));
            dt.Columns.Add("PorcDif2", typeof(decimal));
            dt.Columns.Add("Ajustado", typeof(int));
            dt.Columns.Add("IdUom", typeof(int));
            dt.Columns.Add("Contado1", typeof(decimal));
            dt.Columns.Add("Contado2", typeof(decimal));
            dt.Columns.Add("Contado3", typeof(decimal));
            dt.Columns.Add("Contado4", typeof(decimal));
            dt.Columns.Add("TotalContadoR", typeof(decimal));
            dt.Columns.Add("TotalContadoA", typeof(decimal));
            dt.Columns.Add("DiferenciaContado", typeof(decimal));
            dt.Columns.Add("DiferenciaInventario", typeof(decimal));
            dt.Columns.Add("Modificado1", typeof(int));
            dt.Columns.Add("Modificado2", typeof(int));
            dt.Columns.Add("Modificado3", typeof(int));
            dt.Columns.Add("Modificado4", typeof(int));
            return dt;
        }

        public DBResponse<List<Report_CostoCeroByInventario_Result>> Get_CostoCeroInactivos(int idConfiguracion)
        {
            var response = new DBResponse<List<Report_CostoCeroByInventario_Result>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                dbConfiguracion.ServidorIP,
                                dbConfiguracion.Instancia,
                                dbConfiguracion.Puerto,
                                dbConfiguracion.BaseDatos,
                                dbConfiguracion.Usuario,
                                dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdArticulos = new SqlCommand(Querys_SAP_SQL.Get_SQL_Articulos(), cnnDB);
                        DataTable dtArticulos = new DataTable();
                        SqlDataAdapter daArticulos = new SqlDataAdapter(cmdArticulos);
                        daArticulos.Fill(dtArticulos);
                        if (dtArticulos.Rows.Count > 0)
                        {
                            DataSet ds = new DataSet();
                            ds.DataSetName = "table";
                            DataTable table = new DataTable();
                            table.TableName = "row";
                            table = dtArticulos;
                            ds.Tables.Add(dtArticulos);

                            string xml = ds.GetXml();

                            if (xml == "<table />")
                            {
                                response.Message = "No fue posible consultar la información";
                            }
                            else
                            {
                                var query = db.Report_CostoCeroByInventario(dbConfiguracion.Almacen, xml).ToList();
                                response.ExecutionOK = true;
                                response.Data = query;
                            }

                        }
                        else
                        {
                            response.Message = "No se encontro Productos en el Inventario";
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<List<Listado_DiferenciasMontosUno>> Get_MontosUno(int idConfiguracion)
        {
            var response = new DBResponse<List<Listado_DiferenciasMontosUno>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                dbConfiguracion.ServidorIP,
                                dbConfiguracion.Instancia,
                                dbConfiguracion.Puerto,
                                dbConfiguracion.BaseDatos,
                                dbConfiguracion.Usuario,
                                dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdReporte = new SqlCommand(Querys_SAP_SQL.Get_SQL_DifMontosUno(), cnnDB);
                        DataTable dtReporte = new DataTable();
                        SqlDataAdapter daReporte = new SqlDataAdapter(cmdReporte);
                        daReporte.Fill(dtReporte);

                        if (dtReporte.Rows.Count > 0)
                        {
                            foreach (DataRow rowProcesado in dtReporte.Rows)
                            {
                                string whsCode = rowProcesado[0].ToString();
                                string itmsGrpName = rowProcesado[1].ToString();
                                decimal totalSAP = decimal.Parse(rowProcesado[2].ToString());
                                decimal totalContado = decimal.Parse(rowProcesado[3].ToString());
                                decimal totalDesviacion = decimal.Parse(rowProcesado[4].ToString());
                                decimal porcDif = decimal.Parse(rowProcesado[5].ToString());

                                listado.Add(new Listado_DiferenciasMontosUno
                                {
                                    WhsCode = whsCode,
                                    ItmsGrpName = itmsGrpName,
                                    TotalSAP = totalSAP,
                                    TotalContado = totalContado,
                                    TotalDesviacion = totalDesviacion,
                                    PorcentajeDif = porcDif
                                });
                            }
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();

                        response.ExecutionOK = true;
                        response.Data = listado;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DataTable Get_Procesado(Configuraciones dbConfiguracion)
        {
            DataTable dtProcesado = new DataTable();

            SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                           dbConfiguracion.ServidorIP,
                           dbConfiguracion.Instancia,
                           dbConfiguracion.Puerto,
                           dbConfiguracion.BaseDatos,
                           dbConfiguracion.Usuario,
                           dbConfiguracion.Password));

            cnnDB.Open();

            SqlCommand cmdProcesado = new SqlCommand(Querys_SAP_SQL.Get_SQL_Procesado(), cnnDB);
            SqlDataAdapter daProcesado = new SqlDataAdapter(cmdProcesado);
            daProcesado.Fill(dtProcesado);

            cnnDB.Close();

            return dtProcesado;
        }

        public DBResponse<List<Listado_DiferenciasMontosUno>> Get_MontosDos(int idConfiguracion)
        {
            var response = new DBResponse<List<Listado_DiferenciasMontosUno>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Listado_DiferenciasMontosUno> listado = new List<Listado_DiferenciasMontosUno>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                dbConfiguracion.ServidorIP,
                                dbConfiguracion.Instancia,
                                dbConfiguracion.Puerto,
                                dbConfiguracion.BaseDatos,
                                dbConfiguracion.Usuario,
                                dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdReporte = new SqlCommand(Querys_SAP_SQL.Get_SQL_DifMontosDos(), cnnDB);
                        DataTable dtReporte = new DataTable();
                        SqlDataAdapter daReporte = new SqlDataAdapter(cmdReporte);
                        daReporte.Fill(dtReporte);

                        if (dtReporte.Rows.Count > 0)
                        {
                            foreach (DataRow rowProcesado in dtReporte.Rows)
                            {
                                string whsCode = rowProcesado[0].ToString();
                                string itmsGrpName = rowProcesado[1].ToString();
                                decimal totalSAP = decimal.Parse(rowProcesado[2].ToString());
                                decimal totalContado = decimal.Parse(rowProcesado[3].ToString());
                                decimal totalDesviacion = decimal.Parse(rowProcesado[4].ToString());
                                decimal porcDif = decimal.Parse(rowProcesado[5].ToString());

                                listado.Add(new Listado_DiferenciasMontosUno
                                {
                                    WhsCode = whsCode,
                                    ItmsGrpName = itmsGrpName,
                                    TotalSAP = totalSAP,
                                    TotalContado = totalContado,
                                    TotalDesviacion = totalDesviacion,
                                    PorcentajeDif = porcDif
                                });
                            }
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();

                        response.ExecutionOK = true;
                        response.Data = listado;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<List<Listado_DiferenciasConteoUno>> Get_ConteoUno(int idConfiguracion, string busca, string itmsGrpNam)
        {
            var response = new DBResponse<List<Listado_DiferenciasConteoUno>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Listado_DiferenciasConteoUno> listado = new List<Listado_DiferenciasConteoUno>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        DataTable dtProcesado = Get_Procesado(dbConfiguracion);
                        if (dtProcesado.Rows.Count > 0)
                        {
                            foreach (DataRow rowProcesado in dtProcesado.Rows)
                            {
                                int idDetalle = int.Parse(rowProcesado[0].ToString());
                                string whsCode = rowProcesado[1].ToString();
                                string itmsGrpName = rowProcesado[2].ToString();
                                string itemCode = rowProcesado[3].ToString();
                                string itemName = rowProcesado[4].ToString();
                                int cant1 = Decimal.ToInt32(decimal.Parse(rowProcesado[5].ToString()));
                                int cant2 = Decimal.ToInt32(decimal.Parse(rowProcesado[6].ToString()));
                                decimal totalSAP = decimal.Parse(rowProcesado[7].ToString());
                                decimal totalContado = decimal.Parse(rowProcesado[8].ToString());
                                decimal totalDesviacion = decimal.Parse(rowProcesado[9].ToString());
                                decimal porcDif = decimal.Parse(rowProcesado[10].ToString());
                                string uom = rowProcesado[11].ToString();
                                int onhand = Decimal.ToInt32(decimal.Parse(rowProcesado[12].ToString()));
                                int porProcesar = Decimal.ToInt32(decimal.Parse(rowProcesado[13].ToString()));
                                decimal difPesoNeto = decimal.Parse(rowProcesado[14].ToString());
                                string marbetes = rowProcesado[15].ToString();
                                decimal precio = decimal.Parse(rowProcesado[16].ToString());
                                decimal varianzaNeta = decimal.Parse(rowProcesado[17].ToString());
                                decimal varianzaNetaMonto = decimal.Parse(rowProcesado[18].ToString());

                                decimal totalContado2 = decimal.Parse(rowProcesado[19].ToString());
                                decimal totalDesviacion2 = decimal.Parse(rowProcesado[20].ToString());
                                decimal porcDif2 = decimal.Parse(rowProcesado[21].ToString());

                                listado.Add(new Listado_DiferenciasConteoUno
                                {
                                    WhsCode = whsCode,
                                    ItmsGrpNam = itmsGrpName,
                                    ItemCode = itemCode,
                                    ItemName = itemName,
                                    Uom = uom,
                                    Onhand = onhand,
                                    PorProcesar = porProcesar,
                                    Contado = cant1,
                                    DifPesoNeto = difPesoNeto,
                                    Marbetes = marbetes
                                });
                            }
                        }

                        if (listado != null && listado.Count > 0)
                            listado = listado.Where(w => (String.IsNullOrEmpty(busca) || (!String.IsNullOrEmpty(busca) && (w.ItemCode.Contains(busca) || w.ItemName.Contains(busca))))
                            && (String.IsNullOrEmpty(itmsGrpNam) || (!String.IsNullOrEmpty(itmsGrpNam) && w.ItmsGrpNam.Contains(itmsGrpNam)))).ToList();

                        response.ExecutionOK = true;
                        response.Data = listado;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<List<Listado_DiferenciasAjustes>> Get_AjustesConteoDos(int idConfiguracion, string busca, string itmsGrpNam)
        {
            var response = new DBResponse<List<Listado_DiferenciasAjustes>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Listado_DiferenciasAjustes> listado = new List<Listado_DiferenciasAjustes>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        DataTable dtProcesado = Get_Procesado(dbConfiguracion);
                        if (dtProcesado.Rows.Count > 0)
                        {
                            foreach (DataRow rowProcesado in dtProcesado.Rows)
                            {
                                int idDetalle = int.Parse(rowProcesado[0].ToString());
                                string whsCode = rowProcesado[1].ToString();
                                string itmsGrpName = rowProcesado[2].ToString();
                                string itemCode = rowProcesado[3].ToString();
                                string itemName = rowProcesado[4].ToString();
                                decimal cant1 = decimal.Parse(rowProcesado[5].ToString());
                                decimal cant2 = decimal.Parse(rowProcesado[6].ToString());
                                decimal contado1 = decimal.Parse(rowProcesado[24].ToString());
                                decimal contado2 = decimal.Parse(rowProcesado[25].ToString());
                                decimal contado3 = decimal.Parse(rowProcesado[26].ToString());
                                decimal contado4 = decimal.Parse(rowProcesado[27].ToString());
                                decimal totalSAP = decimal.Parse(rowProcesado[7].ToString());
                                decimal totalContado = decimal.Parse(rowProcesado[8].ToString());
                                decimal totalDesviacion = decimal.Parse(rowProcesado[9].ToString());
                                decimal porcDif = decimal.Parse(rowProcesado[10].ToString());
                                string uom = rowProcesado[11].ToString();
                                decimal onhand = decimal.Parse(rowProcesado[12].ToString());
                                int porProcesar = Decimal.ToInt32(decimal.Parse(rowProcesado[13].ToString()));

                                string marbetes = rowProcesado[15].ToString();
                                decimal precio = decimal.Parse(rowProcesado[16].ToString());
                                decimal varianzaNeta = decimal.Parse(rowProcesado[17].ToString());
                                decimal varianzaNetaMonto = decimal.Parse(rowProcesado[18].ToString());

                                decimal totalContado2 = decimal.Parse(rowProcesado[19].ToString());
                                decimal totalDesviacion2 = decimal.Parse(rowProcesado[20].ToString());
                                decimal porcDif2 = decimal.Parse(rowProcesado[21].ToString());

                                decimal totalContadoR = decimal.Parse(rowProcesado[28].ToString());
                                decimal totalContadoA = decimal.Parse(rowProcesado[29].ToString());
                                decimal diferenciaContado = decimal.Parse(rowProcesado[30].ToString());
                                decimal diferenciaInventario = totalContadoA - onhand; //decimal.Parse(rowProcesado[31].ToString());

                                decimal difPesoNeto = diferenciaInventario * precio; //decimal.Parse(rowProcesado[14].ToString());

                                int modificado1 = int.Parse(rowProcesado[32].ToString());
                                int modificado2 = int.Parse(rowProcesado[33].ToString());
                                int modificado3 = int.Parse(rowProcesado[34].ToString());
                                int modificado4 = int.Parse(rowProcesado[35].ToString());

                                //difPesoNeto = totalDesviacion2 < 0 ? (totalDesviacion2 * -1) : totalDesviacion2;

                                listado.Add(new Listado_DiferenciasAjustes
                                {
                                    IdConfiguracion = dbConfiguracion.IdConfiguracion,
                                    IdDetalle = idDetalle,
                                    WhsCode = whsCode,
                                    ItmsGrpNam = itmsGrpName,
                                    ItemCode = itemCode,
                                    ItemName = itemName,
                                    Uom1 = uom,
                                    Onhand = onhand,
                                    PorProcesar = porProcesar,
                                    Contado = cant2,
                                    Contado1 = contado1,
                                    Contado2 = contado2,
                                    Contado3 = contado3,
                                    Contado4 = contado4,
                                    DifPesosNeto = difPesoNeto,
                                    Precio = precio,
                                    TotalContadoR = totalContadoR,
                                    TotalContadoA = totalContadoA,
                                    DiferenciaContado = diferenciaContado,
                                    DiferenciaInventario = diferenciaInventario,
                                    Modificado1 = modificado1,
                                    Modificado2 = modificado2,
                                    Modificado3 = modificado3,
                                    Modificado4 = modificado4
                                });
                            }
                        }

                        if (listado != null && listado.Count > 0)
                            listado = listado.Where(w => (String.IsNullOrEmpty(busca) || (!String.IsNullOrEmpty(busca) && (w.ItemCode.Contains(busca) || w.ItemName.Contains(busca))))
                            && (String.IsNullOrEmpty(itmsGrpNam) || (!String.IsNullOrEmpty(itmsGrpNam) && w.ItmsGrpNam.Contains(itmsGrpNam)))).ToList();

                        response.ExecutionOK = true;
                        response.Data = listado;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<int> Aply_Ajustes(int idConfiguracion, ConexDIAPI conDiApi, string directorioLogs)
        {
            var response = new DBResponse<int>();
            string txtLog = DateTime.Now.ToString("yyyyMMddHHmmss") + "_AjustesES.log";
            string directorioSeguro = string.IsNullOrEmpty(directorioLogs)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs")
                : directorioLogs;

            try
            {
                if (!Directory.Exists(directorioSeguro)) Directory.CreateDirectory(directorioSeguro);

                using (InventariosEntities db = new InventariosEntities())
                {
                    db.Database.CommandTimeout = 1200;

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion == null)
                    {
                        response.Message = "No se encontró información del Inventario seleccionado";
                        return response;
                    }

                    if (!(dbConfiguracion.Procesado && !dbConfiguracion.Cerrado &&
                          !dbConfiguracion.Deshabilitado && !dbConfiguracion.AjustesAplicados))
                    {
                        response.Message = "El Inventario no está en un estado válido para aplicar ajustes";
                        return response;
                    }

                    // Obtener datos de sobrantes y faltantes en una sola consulta
                    string sqlAjustes = @"
                SELECT 
                    ItemCode, Cant2, Onhand, IdUom, Precio,
                    CASE WHEN Cant2 > Onhand THEN 'Sobrante' ELSE 'Faltante' END AS Tipo
                FROM Procesado
                WHERE Cant2 != Onhand AND Precio > 0";

                    DataTable dtAjustes = new DataTable();
                    using (SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                        dbConfiguracion.ServidorIP, dbConfiguracion.Instancia, dbConfiguracion.Puerto,
                        dbConfiguracion.BaseDatos, dbConfiguracion.Usuario, dbConfiguracion.Password)))
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlAjustes, cnnDB))
                        {
                            cmd.CommandTimeout = 1200;
                            cnnDB.Open();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dtAjustes);
                            }
                        }
                    }

                    if (dtAjustes.Rows.Count == 0)
                    {
                        response.Message = "No se encontraron productos para ajustar";
                        return response;
                    }

                    GrabaLog(directorioSeguro, txtLog, $"Total de ajustes a procesar: {dtAjustes.Rows.Count}");

                    // Conectar a SAP
                    Company oCompany = new Company();
                    oCompany.Server = conDiApi.Server;
                    oCompany.CompanyDB = conDiApi.Company;
                    oCompany.UserName = conDiApi.UserName;
                    oCompany.Password = conDiApi.Password;
                    oCompany.DbUserName = conDiApi.DbUserName;
                    oCompany.DbPassword = conDiApi.DbPassword;
                    oCompany.DbServerType = BoDataServerTypes.dst_HANADB;
                    oCompany.UseTrusted = conDiApi.UseTrusted;

                    int connectionResult = oCompany.Connect();
                    if (connectionResult != 0)
                    {
                        oCompany.GetLastError(out int errorCode, out string errorDescription);
                        response.Message = $"Error al conectar a SAP: {errorCode} - {errorDescription}";
                        return response;
                    }

                    try
                    {
                        var sobrantes = dtAjustes.AsEnumerable().Where(r => r["Tipo"].ToString() == "Sobrante").ToList();
                        var faltantes = dtAjustes.AsEnumerable().Where(r => r["Tipo"].ToString() == "Faltante").ToList();

                        bool todoOK = true;
                        List<string> mensajes = new List<string>();

                        if (sobrantes.Any())
                        {
                            todoOK = ProcesarAjustes(oCompany, sobrantes, dbConfiguracion.Almacen, "Entrada", ref mensajes);
                        }

                        if (todoOK && faltantes.Any())
                        {
                            todoOK = ProcesarAjustes(oCompany, faltantes, dbConfiguracion.Almacen, "Salida", ref mensajes);
                        }

                        if (todoOK)
                        {
                            dbConfiguracion.AjustesAplicados = true;
                            dbConfiguracion.FechaAjustes = DateTime.Now;
                            db.SaveChanges();

                            response.ExecutionOK = true;
                            response.Message = string.Join(" ", mensajes);
                            GrabaLog(directorioSeguro, txtLog, "Proceso completado exitosamente");
                        }
                        else
                        {
                            response.Message = "Error al aplicar ajustes. Revise el log para más detalles.";
                        }
                    }
                    finally
                    {
                        oCompany.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                GrabaLog(directorioSeguro, txtLog, $"Error: {ex.Message}");
            }

            return response;
        }

        private bool ProcesarAjustes(Company oCompany, List<DataRow> items, string almacen, string tipo, ref List<string> mensajes)
        {
            try
            {
                BoObjectTypes docType = tipo == "Entrada"
                    ? BoObjectTypes.oInventoryGenEntry
                    : BoObjectTypes.oInventoryGenExit;

                Documents draft = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                draft.DocObjectCode = docType;
                draft.Comments = $"Ajuste por conteo físico - {tipo} ({DateTime.Now:dd/MM/yyyy HH:mm})";

                int lineIndex = 0;
                foreach (DataRow row in items)
                {
                    string itemCode = row["ItemCode"].ToString();
                    decimal cant2 = Convert.ToDecimal(row["Cant2"]);
                    decimal onhand = Convert.ToDecimal(row["Onhand"]);
                    int uomEntry = Convert.ToInt32(row["IdUom"]);
                    decimal precio = Convert.ToDecimal(row["Precio"]);

                    decimal diferencia = tipo == "Entrada" ? cant2 - onhand : onhand - cant2;

                    if (lineIndex > 0) draft.Lines.Add();

                    draft.Lines.ItemCode = itemCode;
                    draft.Lines.Quantity = (double)diferencia;
                    draft.Lines.UnitPrice = (double)precio;
                    draft.Lines.UoMEntry = uomEntry;
                    draft.Lines.WarehouseCode = almacen;

                    lineIndex++;
                }

                int result = draft.Add();
                if (result != 0)
                {
                    oCompany.GetLastError(out int errCode, out string errMsg);
                    mensajes.Add($"Error en {tipo}: {errCode} - {errMsg}");
                    return false;
                }

                string docEntry = oCompany.GetNewObjectKey();
                mensajes.Add($"Borrador {tipo} creado. DocEntry: {docEntry} ({items.Count} items).");
                return true;
            }
            catch (Exception ex)
            {
                mensajes.Add($"Error procesando {tipo}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Genera un archivo de texto en el servidor y graba el mensaje recibido
        /// </summary>
        public static void GrabaLog(string directory, string file, string mensaje)
        {
            // NUNCA lanzar excepciones - siempre capturar todo
            try
            {
                // Si el directorio está vacío, usar App_Data/Logs
                if (string.IsNullOrEmpty(directory))
                {
                    directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs");
                }

                // Si el nombre del archivo está vacío, generar uno
                if (string.IsNullOrEmpty(file))
                {
                    file = DateTime.Now.ToString("yyyyMMdd") + "_Log.txt";
                }

                // Intentar crear el directorio
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string fullPath = Path.Combine(directory, file);
                string line = DateTime.Now.ToString("HH:mm:ss") + ":::" + mensaje;
                
                // Escribir el archivo
                File.AppendAllText(fullPath, line + Environment.NewLine);
            }
            catch
            {
                // Fallback: escribir en el directorio temporal de Windows
                try
                {
                    string tempPath = Path.GetTempPath();
                    string tempFile = Path.Combine(tempPath, "Blanquita_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                    string line = DateTime.Now.ToString("HH:mm:ss") + ":::" + mensaje;
                    File.AppendAllText(tempFile, line + Environment.NewLine);
                }
                catch
                {
                    // Si todo falla, escribir en Debug (solo para desarrollo)
                    System.Diagnostics.Debug.WriteLine($"LOG: {mensaje}");
                }
            }
        }

        private DataTable Get_Procesado_Sobrante(Configuraciones dbConfiguracion)
        {
            string sql = Querys_SAP_SQL.Get_SQL_Procesado_Sobrante();
            return ExecuteQuery(sql, dbConfiguracion);
        }

        private DataTable Get_Procesado_Faltante(Configuraciones dbConfiguracion)
        {
            string sql = Querys_SAP_SQL.Get_SQL_Procesado_Faltante();
            return ExecuteQuery(sql, dbConfiguracion);
        }

        private DataTable ExecuteQuery(string sql, Configuraciones dbConfiguracion)
        {
            var dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                           dbConfiguracion.ServidorIP,
                           dbConfiguracion.Instancia,
                           dbConfiguracion.Puerto,
                           dbConfiguracion.BaseDatos,
                           dbConfiguracion.Usuario,
                           dbConfiguracion.Password)))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            command.CommandTimeout = 1200;

                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Propagar la excepción para detener el proceso
                throw new Exception($"Error ejecutando consulta SQL: {ex.Message}", ex);
            }

            return dataTable;
        }


        //private DataTable ExecuteQuery(string sql, Configuraciones dbConfiguracion)
        //{
        //    var dataTable = new DataTable();

        //    try
        //    {
        //        using (var connection = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
        //                   dbConfiguracion.ServidorIP,
        //                   dbConfiguracion.Instancia,
        //                   dbConfiguracion.Puerto,
        //                   dbConfiguracion.BaseDatos,
        //                   dbConfiguracion.Usuario,
        //                   dbConfiguracion.Password)))
        //        {                    
        //            connection.Open();                           

        //            using (var adapter = new SqlDataAdapter(sql, connection))
        //            {                        
        //                adapter.Fill(dataTable);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Propagar la excepción para detener el proceso
        //        throw new Exception($"Error ejecutando consulta SQL: {ex.Message}", ex);
        //    }

        //    return dataTable;
        //}

        public DBResponse<List<Listado_ArticulosPDA>> Get_PDAArticulos(int idConfiguracion)
        {
            var response = new DBResponse<List<Listado_ArticulosPDA>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Listado_ArticulosPDA> listado = new List<Listado_ArticulosPDA>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdDatos = new SqlCommand(Querys_SAP_SQL.Get_SQL_PDAArticulos(), cnnDB);
                        DataTable dtDatos = new DataTable();
                        SqlDataAdapter daDatos = new SqlDataAdapter(cmdDatos);
                        daDatos.Fill(dtDatos);
                        if (dtDatos.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtDatos.Rows)
                            {
                                listado.Add(new Listado_ArticulosPDA
                                {
                                    PDA = row[0].ToString(),
                                    Cantidad = int.Parse(row[1].ToString())
                                });
                            }
                        }

                        if (cnnDB.State == ConnectionState.Open)
                            cnnDB.Close();

                        response.ExecutionOK = true;
                        response.Data = listado;
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<int> Update_AjustesConteoDos(int idConfiguracion, List<Listado_AjustesConteoDos> listadoAjustes)
        {
            var response = new DBResponse<int>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        if (dbConfiguracion.Activo == true && dbConfiguracion.Cerrado == false && dbConfiguracion.Deshabilitado == false && dbConfiguracion.Procesado == true && dbConfiguracion.AjustesAplicados == false)
                        {
                            SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                                                dbConfiguracion.ServidorIP,
                                                dbConfiguracion.Instancia,
                                                dbConfiguracion.Puerto,
                                                dbConfiguracion.BaseDatos,
                                                dbConfiguracion.Usuario,
                                                dbConfiguracion.Password));
                            cnnDB.Open();

                            foreach (var itemAjuste in listadoAjustes)
                            {
                                SqlCommand cmdItemProcesado = new SqlCommand(Querys_SAP_SQL.Get_SQL_ProductoProcesado(), cnnDB);
                                cmdItemProcesado.Parameters.AddWithValue("@IdDetalle", itemAjuste.IdDetalle);

                                DataTable dtItemProcesado = new DataTable();
                                SqlDataAdapter daItemProcesado = new SqlDataAdapter(cmdItemProcesado);
                                daItemProcesado.Fill(dtItemProcesado);
                                if (dtItemProcesado.Rows.Count > 0)
                                {
                                    int ajustado = int.Parse(dtItemProcesado.Rows[0][22].ToString());
                                    if (ajustado == 0)
                                    {
                                        decimal totalSAP = decimal.Parse(dtItemProcesado.Rows[0][7].ToString());
                                        decimal onhand = decimal.Parse(dtItemProcesado.Rows[0][12].ToString());
                                        decimal precio = decimal.Parse(dtItemProcesado.Rows[0][16].ToString());

                                        decimal totalContado2 = itemAjuste.Cantidad * precio;
                                        decimal totalDesviacion2 = totalContado2 - totalSAP;
                                        decimal totalPorcDif2 = totalSAP != 0 ? (totalDesviacion2 / totalSAP * 100) : 0;
                                        decimal varianzaNeta = itemAjuste.Cantidad - onhand;
                                        decimal varianzaNetaMonto = varianzaNeta >= 0 ? (varianzaNeta * precio) : ((varianzaNeta * -1) * precio);

                                        decimal contado = decimal.Parse(dtItemProcesado.Rows[0][6].ToString());
                                        decimal totalContadoR = itemAjuste.Cantidad;
                                        decimal totalContadoA = itemAjuste.Cantidad > 0 ? itemAjuste.Cantidad : contado;
                                        decimal diferenciaContado = contado - itemAjuste.Cantidad;
                                        decimal diferenciaInventario = totalContadoA - onhand;

                                        decimal difPesoNeto = precio * diferenciaInventario;

                                        SqlCommand cmdUpdateAjusteConteoDos = new SqlCommand(Querys_SAP_SQL.Update_SQL_AjusteConteoDos(), cnnDB);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@IdDetalle", itemAjuste.IdDetalle);
                                        //cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Cant2", itemAjuste.Cantidad);
                                        //cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@VarianzaNeta", varianzaNeta);
                                        //cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@VarianzaNetaMonto", varianzaNetaMonto);
                                        //cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@TotalContado2", totalContado2);
                                        //cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@TotalDesviacion2", totalDesviacion2);
                                        //cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@PorcDif2", totalPorcDif2);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Contado1", itemAjuste.Contado1);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Contado2", itemAjuste.Contado2);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Contado3", itemAjuste.Contado3);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Contado4", itemAjuste.Contado4);

                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@TotalContadoR", totalContadoR);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@TotalContadoA", totalContadoA);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@DiferenciaContado", diferenciaContado);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@DiferenciaInventario", diferenciaInventario);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Modificado1", itemAjuste.Contado1 > 0 ? 1 : 0);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Modificado2", itemAjuste.Contado2 > 0 ? 1 : 0);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Modificado3", itemAjuste.Contado3 > 0 ? 1 : 0);
                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@Modificado4", itemAjuste.Contado4 > 0 ? 1 : 0);

                                        cmdUpdateAjusteConteoDos.Parameters.AddWithValue("@DifPesoNeto", difPesoNeto);

                                        cmdUpdateAjusteConteoDos.ExecuteNonQuery();
                                    }
                                }
                            }


                            if (cnnDB.State == ConnectionState.Open)
                                cnnDB.Close();

                            response.ExecutionOK = true;
                            response.Message = "Se aplicaron los ajustes";
                        }
                        else
                        {
                            response.Message = "No fue posible aplicar los ajustes debido a que el estatus actual del Inventario lo impide";
                        }
                    }
                    else
                    {
                        response.Message = "No se encontro información del Inventario";
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

        public DBResponse<List<Diferencias_Reporte>> Get_DiferenciasReporte(decimal idConfiguracion, decimal montoVarianza, string categoria)
        {
            var response = new DBResponse<List<Diferencias_Reporte>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Diferencias_Reporte> listadoContado = new List<Diferencias_Reporte>();
                    List<Diferencias_Reporte> listadoProcesado = new List<Diferencias_Reporte>();
                    List<Diferencias_Reporte> listadoResultado = new List<Diferencias_Reporte>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdDatos = new SqlCommand(Querys_SAP_SQL.Get_ProcesadoReporte2(), cnnDB);
                        cmdDatos.Parameters.AddWithValue("@ItmsGrpNam", categoria);
                        cmdDatos.Parameters.AddWithValue("@VarianzaNetaMonto", montoVarianza);

                        DataTable dtDatos = new DataTable();
                        SqlDataAdapter daDatos = new SqlDataAdapter(cmdDatos);
                        daDatos.Fill(dtDatos);
                        if (dtDatos.Rows.Count > 0)
                        {
                            foreach (DataRow rowProcesado in dtDatos.Rows)
                            {
                                listadoProcesado.Add(new Diferencias_Reporte
                                {
                                    IdZona = 0,
                                    Zona = "SIN ZONA",
                                    CodigoArticulo = rowProcesado[3].ToString(),
                                    NombreArticulo = rowProcesado[4].ToString(),
                                    Categoria = rowProcesado[2].ToString(),
                                    Uom1 = rowProcesado[11].ToString(),
                                    Onhand = decimal.Parse(rowProcesado[12].ToString()),
                                    Contado = decimal.Parse(rowProcesado[5].ToString()),
                                    Precio = decimal.Parse(rowProcesado[16].ToString()),
                                    VNetaPza = decimal.Parse(rowProcesado[17].ToString()),
                                    VNetaMonto = decimal.Parse(rowProcesado[18].ToString()),
                                });
                            }

                            SqlCommand cmdDatosZona = new SqlCommand(Querys_SAP_SQL.Get_Reporte2ProductosZona(), cnnDB);
                            cmdDatosZona.Parameters.AddWithValue("@ItmsGrpNam", categoria);

                            DataTable dtDatosZona = new DataTable();
                            SqlDataAdapter daDatosZona = new SqlDataAdapter(cmdDatosZona);
                            daDatosZona.Fill(dtDatosZona);
                            if (dtDatosZona.Rows.Count > 0)
                            {
                                foreach (DataRow rowCapturado in dtDatosZona.Rows)
                                {
                                    listadoContado.Add(new Diferencias_Reporte
                                    {
                                        IdZona = int.Parse(rowCapturado[0].ToString()),
                                        CodigoArticulo = rowCapturado[1].ToString(),
                                        NombreArticulo = rowCapturado[2].ToString(),
                                        Categoria = "",
                                        Uom1 = "",
                                        Onhand = 0,
                                        Contado = decimal.Parse(rowCapturado[3].ToString()),
                                        Precio = decimal.Parse(rowCapturado[4].ToString()),
                                        VNetaPza = 0,
                                        VNetaMonto = 0,
                                    });
                                }
                            }

                            if (listadoContado != null && listadoContado.Count > 0)
                            {
                                foreach (var item in listadoProcesado.Where(w => w.Contado > 0).ToList())
                                {
                                    List<Diferencias_Reporte> datosContado = listadoContado.Where(w => w.CodigoArticulo == item.CodigoArticulo).ToList();
                                    string zona = "";
                                    string marbetes = "";

                                    if (datosContado != null && datosContado.Count > 0)
                                    {
                                        foreach (var contado in datosContado)
                                        {
                                            var dbZona = db.Zonas.Find(contado.IdZona);
                                            zona = dbZona.Zona;

                                            SqlCommand cmdMarbetes = new SqlCommand(Querys_SAP_SQL.Get_Reporte2ProductoZonaMarbetes(), cnnDB);
                                            cmdMarbetes.Parameters.AddWithValue("@CodigoArticulo", contado.CodigoArticulo);
                                            cmdMarbetes.Parameters.AddWithValue("@IdZona", contado.IdZona);

                                            DataTable dtMarbetes = new DataTable();
                                            SqlDataAdapter daMarbetes = new SqlDataAdapter(cmdMarbetes);
                                            daMarbetes.Fill(dtMarbetes);
                                            if (dtMarbetes.Rows.Count > 0)
                                            {
                                                marbetes = dtMarbetes.Rows[0][0].ToString();
                                            }

                                            listadoResultado.Add(new Diferencias_Reporte
                                            {
                                                IdZona = contado.IdZona,
                                                Zona = zona,
                                                Marbetes = marbetes,
                                                CodigoArticulo = contado.CodigoArticulo,
                                                NombreArticulo = contado.NombreArticulo,
                                                Categoria = item.Categoria,
                                                Uom1 = item.Uom1,
                                                Onhand = item.Onhand,
                                                Contado = contado.Contado,
                                                Precio = contado.Precio,
                                                VNetaPza = item.VNetaPza,
                                                VNetaMonto = item.VNetaMonto,
                                            });
                                        }
                                    }
                                }
                            }

                            listadoResultado.AddRange(listadoProcesado.Where(w => w.Contado == 0).ToList());
                        }
                    }

                    response.ExecutionOK = true;
                    response.Data = listadoResultado;
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

        public DBResponse<Zonas> Search_ZonaByIdConfiguracionAndIdZona(int idConfiguracion, int idZona)
        {
            var response = new DBResponse<Zonas>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var dbZona = (from i in db.Zonas
                                  where i.IdConfiguracion == idConfiguracion
                                    && i.IdZona == idZona
                                  select i).FirstOrDefault();
                    if (dbZona != null)
                    {
                        response.ExecutionOK = true;
                        response.Data = dbZona;
                    }
                    else
                    {
                        response.Message = "No se encontro la información necesaria para generar el reporte, revise la Zona";
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

        public DBResponse<List<Diferencias_Reporte>> Get_DiferenciasReporteZona(decimal idConfiguracion, decimal montoVarianza, int idZona)
        {
            var response = new DBResponse<List<Diferencias_Reporte>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    List<Diferencias_Reporte> listadoContado = new List<Diferencias_Reporte>();
                    List<Diferencias_Reporte> listadoProcesado = new List<Diferencias_Reporte>();
                    List<Diferencias_Reporte> listadoResultado = new List<Diferencias_Reporte>();

                    var dbConfiguracion = db.Configuraciones.Find(idConfiguracion);
                    if (dbConfiguracion != null)
                    {
                        SqlConnection cnnDB = new SqlConnection(Querys_SAP_SQL.Get_SQL_Connection(
                            dbConfiguracion.ServidorIP,
                            dbConfiguracion.Instancia,
                            dbConfiguracion.Puerto,
                            dbConfiguracion.BaseDatos,
                            dbConfiguracion.Usuario,
                            dbConfiguracion.Password));

                        cnnDB.Open();

                        SqlCommand cmdDatos = new SqlCommand(Querys_SAP_SQL.Get_ProcesadoReporteBaseZona(), cnnDB);
                        cmdDatos.Parameters.AddWithValue("@VarianzaNetaMonto", montoVarianza);

                        DataTable dtDatos = new DataTable();
                        SqlDataAdapter daDatos = new SqlDataAdapter(cmdDatos);
                        daDatos.Fill(dtDatos);
                        if (dtDatos.Rows.Count > 0)
                        {
                            foreach (DataRow rowProcesado in dtDatos.Rows)
                            {
                                listadoProcesado.Add(new Diferencias_Reporte
                                {
                                    IdZona = 0,
                                    Zona = "SIN ZONA",
                                    CodigoArticulo = rowProcesado[3].ToString(),
                                    NombreArticulo = rowProcesado[4].ToString(),
                                    Categoria = rowProcesado[2].ToString(),
                                    Uom1 = rowProcesado[11].ToString(),
                                    Onhand = decimal.Parse(rowProcesado[12].ToString()),
                                    Contado = decimal.Parse(rowProcesado[5].ToString()),
                                    Precio = decimal.Parse(rowProcesado[16].ToString()),
                                    VNetaPza = decimal.Parse(rowProcesado[17].ToString()),
                                    VNetaMonto = decimal.Parse(rowProcesado[18].ToString()),
                                });
                            }

                            SqlCommand cmdDatosZona = new SqlCommand(Querys_SAP_SQL.Get_Reporte2ProductosZonaByIdZona(), cnnDB);
                            cmdDatosZona.Parameters.AddWithValue("@IdZona", idZona);

                            DataTable dtDatosZona = new DataTable();
                            SqlDataAdapter daDatosZona = new SqlDataAdapter(cmdDatosZona);
                            daDatosZona.Fill(dtDatosZona);
                            if (dtDatosZona.Rows.Count > 0)
                            {
                                foreach (DataRow rowCapturado in dtDatosZona.Rows)
                                {
                                    listadoContado.Add(new Diferencias_Reporte
                                    {
                                        IdZona = int.Parse(rowCapturado[0].ToString()),
                                        CodigoArticulo = rowCapturado[1].ToString(),
                                        NombreArticulo = rowCapturado[2].ToString(),
                                        Categoria = "",
                                        Uom1 = "",
                                        Onhand = 0,
                                        Contado = decimal.Parse(rowCapturado[3].ToString()),
                                        Precio = decimal.Parse(rowCapturado[4].ToString()),
                                        VNetaPza = 0,
                                        VNetaMonto = 0,
                                    });
                                }
                            }

                            if (listadoContado != null && listadoContado.Count > 0)
                            {
                                foreach (var item in listadoProcesado.Where(w => w.Contado > 0).ToList())
                                {
                                    List<Diferencias_Reporte> datosContado = listadoContado.Where(w => w.CodigoArticulo == item.CodigoArticulo).ToList();
                                    string zona = "";
                                    string marbetes = "";

                                    if (datosContado != null && datosContado.Count > 0)
                                    {
                                        foreach (var contado in datosContado)
                                        {
                                            var dbZona = db.Zonas.Find(contado.IdZona);
                                            zona = dbZona.Zona;

                                            SqlCommand cmdMarbetes = new SqlCommand(Querys_SAP_SQL.Get_Reporte2ProductoZonaMarbetes(), cnnDB);
                                            cmdMarbetes.Parameters.AddWithValue("@CodigoArticulo", contado.CodigoArticulo);
                                            cmdMarbetes.Parameters.AddWithValue("@IdZona", contado.IdZona);

                                            DataTable dtMarbetes = new DataTable();
                                            SqlDataAdapter daMarbetes = new SqlDataAdapter(cmdMarbetes);
                                            daMarbetes.Fill(dtMarbetes);
                                            if (dtMarbetes.Rows.Count > 0)
                                            {
                                                marbetes = dtMarbetes.Rows[0][0].ToString();
                                            }

                                            listadoResultado.Add(new Diferencias_Reporte
                                            {
                                                IdZona = contado.IdZona,
                                                Zona = zona,
                                                Marbetes = marbetes,
                                                CodigoArticulo = contado.CodigoArticulo,
                                                NombreArticulo = contado.NombreArticulo,
                                                Categoria = item.Categoria,
                                                Uom1 = item.Uom1,
                                                Onhand = item.Onhand,
                                                Contado = contado.Contado,
                                                Precio = contado.Precio,
                                                VNetaPza = item.VNetaPza,
                                                VNetaMonto = item.VNetaMonto,
                                            });
                                        }
                                    }
                                }
                            }

                            listadoResultado.AddRange(listadoProcesado.Where(w => w.Contado == 0).ToList());
                        }
                    }

                    response.ExecutionOK = true;
                    response.Data = listadoResultado.Where(w => w.IdZona == idZona).OrderByDescending(o => o.VNetaMonto).ToList();
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
