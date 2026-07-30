using Blanquita_Inventarios.AppMAUI.Service;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Android.Bluetooth;
using Microsoft.Maui.Devices;
#endif


namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class ItemMarbeteVM : ObservableObject
    {
        private ApiService apiService;
        private readonly DatabaseService _database;

        [ObservableProperty]
        int idZona;

        [ObservableProperty]
        int marbete;

        [ObservableProperty]
        string estatus;

        [ObservableProperty]
        int cantidadProductos;

        [ObservableProperty]
        bool visible;

        [ObservableProperty]
        bool permiteDescargar;

        public ItemMarbeteVM()
        {
            apiService = new ApiService();
            _database = new DatabaseService(FileSystem.AppDataDirectory);
            CantidadProductos = 0;
            PermiteDescargar = true;
        }

        [RelayCommand]
        async void Abrir()
        {
            var result = await Shell.Current.DisplayAlert("", "¿Desea abrir el Marbete seleccionado?", "OK", "Cancelar");
            if (result)
            {
                var resultCodigo = await Shell.Current.DisplayPromptAsync("", "Ingrese el Código de autorización");
                if (!String.IsNullOrEmpty(resultCodigo))
                {
                    int idUsuario = 0;
                    if (Preferences.ContainsKey("User_ID"))
                        idUsuario = Preferences.Get("User_ID", 0);

                    int idConfiguracion = 0;
                    if (Preferences.ContainsKey("IdConfiguracion"))
                        idConfiguracion = Preferences.Get("IdConfiguracion", 0);

                    UseCodigo usar = new UseCodigo
                    {
                        IdConfiguracion = idConfiguracion,
                        Codigo = resultCodigo,
                        IdAccion = 2,
                        IdUsuario = idUsuario
                    };

                    var urlApi = App.Current.Resources["UrlAPI"].ToString();
                    var responseUsarCodigo = await apiService.PostObj<int>(urlApi, "/api", "/UsarCodigo", usar);
                    if (responseUsarCodigo.ExecutionOK)
                    {
                        var response = await _database.Abrir_Marbete(Marbete);
                        if (response.ExecutionOK)
                        {
                            await Shell.Current.DisplayAlert("", "El Marbete ha sido Abierto", "OK");
                            Visible = false;
                            Estatus = "Abierto";
                        }
                        else
                            await Shell.Current.DisplayAlert("", response.Message, "OK");
                    }
                }
            }
        }

        private string ObtenerNombreBluetooth()
        {
#if ANDROID
            try
            {
                var adapter = BluetoothAdapter.DefaultAdapter;

                if (adapter != null && !string.IsNullOrWhiteSpace(adapter.Name))
                    return adapter.Name;

                return $"{DeviceInfo.Manufacturer}-{DeviceInfo.Model}";
            }
            catch
            {
                return $"{DeviceInfo.Manufacturer}-{DeviceInfo.Model}";
            }
#else
    return "PDA";
#endif
        }


        [RelayCommand]
        async void Descargar()
        {
            try
            {
                string nombrePda = ObtenerNombreBluetooth();

                PermiteDescargar = false;
                UserDialogs.Instance.ShowLoading("Espere un momento...");

                var response = await _database.Get_MarbeteProductos(Marbete);
                if (response.ExecutionOK)
                {
                    var listado = response.Data;
                    if (listado != null && listado.Count > 0)
                    {
                        int idConfiguracion = 0;
                        if (Preferences.ContainsKey("IdConfiguracion"))
                            idConfiguracion = Preferences.Get("IdConfiguracion", 0);

                        if (idConfiguracion > 0)
                        {
                            var urlApi = App.Current.Resources["UrlAPI"].ToString();
                            var responseConfiguracion = await apiService.Get<Configuraciones>(urlApi, "/api", "/GetConfiguracion", idConfiguracion);

                            if (responseConfiguracion.ExecutionOK)
                            {
                                Configuraciones configuracion = responseConfiguracion.Data;

                                //Realizamos la conexion a la base de datos del inventario
                                SqlConnection cnnDB = new SqlConnection(
                                    Querys_SAP_SQL.Get_SQL_Connection(
                                        configuracion.ServidorIP,
                                        configuracion.Instancia,
                                        configuracion.Puerto,
                                        configuracion.BaseDatos,
                                        configuracion.Usuario,
                                        configuracion.Password));
                                cnnDB.Open();

                                try
                                {
                                    var responseMarbete = await _database.Get_Marbete(Marbete);
                                    if (responseMarbete.ExecutionOK)
                                    {
                                        if (responseMarbete.Data != null)
                                        {
                                            SI_Marbetes_Sqlite dbMarbete = responseMarbete.Data;

                                            SqlCommand cmdMarbete = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteById(), cnnDB);
                                            cmdMarbete.Parameters.AddWithValue("@IdMarbete", dbMarbete.IdMarbete);

                                            DataTable dtMarbete = new DataTable();
                                            SqlDataAdapter daMarbete = new SqlDataAdapter(cmdMarbete);
                                            daMarbete.Fill(dtMarbete);
                                            if (dtMarbete.Rows.Count > 0)
                                            {
                                                int estatus = int.Parse(dtMarbete.Rows[0][2].ToString());
                                                if (estatus != 3)
                                                {
                                                    SqlTransaction transaction = cnnDB.BeginTransaction();
                                                    SqlCommand cmdDB = cnnDB.CreateCommand();
                                                    cmdDB.Connection = cnnDB;
                                                    cmdDB.Transaction = transaction;

                                                    int totalItemsMarbete = listado.Count;
                                                    bool marbeteDescargado = false;
                                                    for (int intento = 1; intento <= 3; intento++)
                                                    {
                                                        foreach (var item in listado)
                                                        {
                                                            string capturo = item.Capturo != null ? item.Capturo : nombrePda;

                                                            //Query para validar si esta registrado el articulo en SQL
                                                            cmdDB.CommandText = Querys_SAP_SQL.Search_SQL_ExisteArticuloRegistrado();
                                                            cmdDB.Parameters.Clear();
                                                            cmdDB.Parameters.AddWithValue("@IdMarbete", item.IdMarbete);
                                                            cmdDB.Parameters.AddWithValue("@ItemCode", item.Codigo);
                                                            cmdDB.Parameters.AddWithValue("@Capturo", capturo);
                                                            cmdDB.Parameters.AddWithValue("@Fecha", item.FechaRegistro);
                                                            int totalRegistros = (int)cmdDB.ExecuteScalar();

                                                            if (totalRegistros == 0)
                                                            {
                                                                cmdDB.CommandText = Querys_SAP_SQL.Insert_MarbeteArticulos();
                                                                cmdDB.Parameters.Clear();
                                                                cmdDB.Parameters.AddWithValue("@IdMarbete", item.IdMarbete);
                                                                cmdDB.Parameters.AddWithValue("@CodigoArticulo", item.Codigo);
                                                                cmdDB.Parameters.AddWithValue("@CodigoBarras", item.Barcode);
                                                                cmdDB.Parameters.AddWithValue("@Descripcion", item.Descripcion);
                                                                cmdDB.Parameters.AddWithValue("@Grupo", "");
                                                                cmdDB.Parameters.AddWithValue("@Uom", item.Uom);
                                                                cmdDB.Parameters.AddWithValue("@BaseQty", item.BaseQty);
                                                                cmdDB.Parameters.AddWithValue("@Precio", item.Precio);
                                                                cmdDB.Parameters.AddWithValue("@Costo", item.Costo);
                                                                cmdDB.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                                                                cmdDB.Parameters.AddWithValue("@Cantidad2", item.Cantidad);
                                                                cmdDB.Parameters.AddWithValue("@Capturo", capturo);
                                                                cmdDB.Parameters.AddWithValue("@FechaCaptura", item.FechaRegistro);
                                                                cmdDB.ExecuteNonQuery();
                                                            }
                                                        }

                                                        int idMarbeteEvaluado = listado.Select(s => s.IdMarbete).FirstOrDefault();
                                                        //Query para revisar la cantidad de articuos registrados en la BS del Inventario
                                                        cmdDB.CommandText = Querys_SAP_SQL.Get_SQL_TotalArticulosByMarbete();
                                                        cmdDB.Parameters.Clear();
                                                        cmdDB.Parameters.AddWithValue("@IdMarbete", idMarbeteEvaluado);

                                                        int totalRegistrados = (int)cmdDB.ExecuteScalar();
                                                        if (totalItemsMarbete == totalRegistrados)
                                                        {
                                                            string capturoMarbete = dbMarbete.Capturo != null ? dbMarbete.Capturo : nombrePda;

                                                            cmdDB.CommandText = Querys_SAP_SQL.Update_MarbeteCerrado();
                                                            cmdDB.Parameters.Clear();
                                                            cmdDB.Parameters.AddWithValue("@Capturo", capturoMarbete);
                                                            cmdDB.Parameters.AddWithValue("@Inicio", dbMarbete.Inicio);
                                                            cmdDB.Parameters.AddWithValue("@FechaCierre", dbMarbete.Fin);
                                                            cmdDB.Parameters.AddWithValue("@IdMarbete", dbMarbete.IdMarbete);
                                                            cmdDB.ExecuteNonQuery();

                                                            await _database.Update_MarbeteDescargado(Marbete);
                                                            Visible = false;
                                                            Estatus = "Enviado";

                                                            transaction.Commit();

                                                            var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
                                                            if (parametrizacion.Count > 0)
                                                            {
                                                                var parametros = parametrizacion.Where(s => s.IdParametrizacion == 1).FirstOrDefault();
                                                                if (parametros != null)
                                                                {
                                                                    int marbetesDescargados = 0;
                                                                    var responseMD = await _database.Get_MarbetesDescargados();
                                                                    if (responseMD.ExecutionOK)
                                                                        marbetesDescargados = responseMD.Data.Count();

                                                                    parametros.Descargados = marbetesDescargados;
                                                                    await _database.Update_Parametrizacion(parametros);
                                                                    Preferences.Set("Descargados", parametros.Descargados);
                                                                }
                                                            }

                                                            marbeteDescargado = true;

                                                            UserDialogs.Instance.HideHud();
                                                            await Shell.Current.DisplayAlert("", "El Marbete fue enviado correctamente", "OK");

                                                            break;
                                                        }
                                                    }

                                                    if (!marbeteDescargado)
                                                    {
                                                        transaction.Rollback();

                                                        UserDialogs.Instance.HideHud();
                                                        await Shell.Current.DisplayAlert("", "Ocurrio un problema al intentar descargar la información del Marbete", "OK");
                                                    }
                                                }
                                                else
                                                {
                                                    UserDialogs.Instance.HideHud();
                                                    await Shell.Current.DisplayAlert("", "No se puede descargar el Marbete porque esta cerrado en la Base de datos del Inventario", "OK");
                                                }
                                            }
                                            else
                                            {
                                                UserDialogs.Instance.HideHud();
                                                await Shell.Current.DisplayAlert("", "No fue posible validar el Marbete o no se encuentra en la Base de datos del Inventario", "OK");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        UserDialogs.Instance.HideHud();
                                        await Shell.Current.DisplayAlert("", "Ocurrio un problema al intentar obtener la información del Marbete", "OK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    UserDialogs.Instance.HideHud();
                                    await Shell.Current.DisplayAlert("", ex.Message, "OK");
                                }
                                finally
                                {
                                    if (cnnDB.State == System.Data.ConnectionState.Open)
                                        cnnDB.Close();
                                }
                            }
                            else
                            {
                                UserDialogs.Instance.HideHud();
                                await Shell.Current.DisplayAlert("", responseConfiguracion.Message, "OK");
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideHud();
                            await Shell.Current.DisplayAlert("", "No es posible descargar la información ya que no se encontro datos de la conexión a la BD", "OK");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideHud();
                        await Shell.Current.DisplayAlert("", "No se encontraron productos para descargar", "OK");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "No se encontro información para descargar", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }

            PermiteDescargar = true;
        }
    }
}
