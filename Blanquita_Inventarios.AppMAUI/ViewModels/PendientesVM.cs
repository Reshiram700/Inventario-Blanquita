using Blanquita_Inventarios.AppMAUI.Generics;
using Blanquita_Inventarios.AppMAUI.Service;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Android.Bluetooth;
#endif


namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class PendientesVM : ObservableObject
    {
        private ApiService apiService;
        private readonly DatabaseService _database;

        [ObservableProperty]
        ObservableCollection<ItemMarbeteVM> getMarbetes;

        [ObservableProperty]
        int articulos;

        [ObservableProperty]
        bool permiteDescargar;

        public PendientesVM()
        {
            apiService = new ApiService();
            _database = new DatabaseService(FileSystem.AppDataDirectory);

            PermiteDescargar = true;
            Articulos = 0;

            Carga_Marbetes();
        }

        private async void Carga_Marbetes()
        {
            var response = await _database.Get_MarbetesPendientesDescargar();
            if (response.ExecutionOK)
            {
                var list = response.Data.Select(a => new ItemMarbeteVM
                {
                    IdZona = a.IdZona,                    
                    Marbete = a.Marbete,
                    Estatus = "Pendiente de Enviar",
                    CantidadProductos = a.CantidadProductos,
                    Visible = true
                }).ToList();

                Articulos = list.Sum(a => a.CantidadProductos);

                GetMarbetes = new ObservableCollection<ItemMarbeteVM>(list);
            }
        }

        private string ObtenerNombreBluetooth()
        {
#if ANDROID
            try
            {
                var adapter = BluetoothAdapter.DefaultAdapter;

                if (adapter != null)
                {
                    var nombre = adapter.Name;

                    if (!string.IsNullOrWhiteSpace(nombre))
                        return nombre;
                }

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
                // Validación inicial - si no hay marbetes, salir inmediatamente
                if (GetMarbetes == null || GetMarbetes.Count == 0)
                {
                    await Shell.Current.DisplayAlert("", "No hay Marbetes Pendientes por enviar", "OK");
                    return;
                }

                string nombrePda = ObtenerNombreBluetooth();
                PermiteDescargar = false;
                bool todoOk = true;
                string msjError = "";
                string msjErrorCerrado = "";

                UserDialogs.Instance.ShowLoading("Espere un momento...");

                var response = await _database.Get_MarbetesPendientesDescargar();
                if (!response.ExecutionOK)
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "Error al obtener los marbetes pendientes", "OK");
                    PermiteDescargar = true;
                    return;
                }

                var listadoMarbetes = response.Data;

                // Validación adicional - verificar que hay datos después de la consulta
                if (listadoMarbetes == null || listadoMarbetes.Count == 0)
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "No hay Marbetes Pendientes por enviar", "OK");
                    PermiteDescargar = true;
                    Carga_Marbetes(); // Refrescar por si acaso
                    return;
                }

                int idConfiguracion = 0;
                if (Preferences.ContainsKey("IdConfiguracion"))
                    idConfiguracion = Preferences.Get("IdConfiguracion", 0);

                if (idConfiguracion <= 0)
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "No es posible establecer una conexión a la Base de Datos del Inventario", "OK");
                    PermiteDescargar = true;
                    return;
                }

                var urlApi = App.Current.Resources["UrlAPI"].ToString();
                var responseConfiguracion = await apiService.Get<Configuraciones>(urlApi, "/api", "/GetConfiguracion", idConfiguracion);

                if (!responseConfiguracion.ExecutionOK)
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "Ocurrió un problema al intentar obtener los datos de conexión", "OK");
                    PermiteDescargar = true;
                    return;
                }

                Configuraciones configuracion = responseConfiguracion.Data;
                SqlConnection cnnDB = null;

                try
                {
                    //Realizamos la conexion a la base de datos del inventario
                    cnnDB = new SqlConnection(
                        Querys_SAP_SQL.Get_SQL_Connection(
                            configuracion.ServidorIP,
                            configuracion.Instancia,
                            configuracion.Puerto,
                            configuracion.BaseDatos,
                            configuracion.Usuario,
                            configuracion.Password));
                    cnnDB.Open();

                    foreach (var marbete in listadoMarbetes)
                    {
                        SqlCommand cmdMarbete = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteById(), cnnDB);
                        cmdMarbete.Parameters.AddWithValue("@IdMarbete", marbete.IdMarbete);

                        DataTable dtMarbete = new DataTable();
                        SqlDataAdapter daMarbete = new SqlDataAdapter(cmdMarbete);
                        daMarbete.Fill(dtMarbete);

                        if (dtMarbete.Rows.Count > 0)
                        {
                            int estatus = int.Parse(dtMarbete.Rows[0][2].ToString());
                            if (estatus != 3)
                            {
                                var responseProductos = await _database.Get_MarbeteProductos(marbete.Marbete);
                                if (responseProductos.ExecutionOK)
                                {
                                    SqlTransaction transaction = cnnDB.BeginTransaction();
                                    SqlCommand cmdDB = cnnDB.CreateCommand();
                                    cmdDB.Connection = cnnDB;
                                    cmdDB.Transaction = transaction;

                                    var listadoItems = responseProductos.Data;
                                    if (listadoItems != null && listadoItems.Count > 0)
                                    {
                                        int totalItemsMarbete = listadoItems.Count;
                                        bool marbeteDescargado = false;

                                        for (int intento = 1; intento <= 3; intento++)
                                        {
                                            foreach (var item in listadoItems)
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

                                            int idMarbeteEvaluado = listadoItems.Select(s => s.IdMarbete).FirstOrDefault();
                                            //Query para revisar la cantidad de articuos registrados en la BS del Inventario
                                            cmdDB.CommandText = Querys_SAP_SQL.Get_SQL_TotalArticulosByMarbete();
                                            cmdDB.Parameters.Clear();
                                            cmdDB.Parameters.AddWithValue("@IdMarbete", idMarbeteEvaluado);

                                            int totalRegistrados = (int)cmdDB.ExecuteScalar();
                                            if (totalItemsMarbete == totalRegistrados)
                                            {
                                                var responseMarbete = await _database.Get_Marbete(marbete.Marbete);
                                                if (responseMarbete.ExecutionOK)
                                                {
                                                    if (responseMarbete.Data != null)
                                                    {
                                                        SI_Marbetes_Sqlite dbMarbete = responseMarbete.Data;

                                                        string capturoMarbete = dbMarbete.Capturo != null ? dbMarbete.Capturo : nombrePda;

                                                        cmdDB.CommandText = Querys_SAP_SQL.Update_MarbeteCerrado();
                                                        cmdDB.Parameters.Clear();
                                                        cmdDB.Parameters.AddWithValue("@Capturo", capturoMarbete);
                                                        cmdDB.Parameters.AddWithValue("@Inicio", dbMarbete.Inicio);
                                                        cmdDB.Parameters.AddWithValue("@FechaCierre", dbMarbete.Fin);
                                                        cmdDB.Parameters.AddWithValue("@IdMarbete", dbMarbete.IdMarbete);
                                                        cmdDB.ExecuteNonQuery();

                                                        await _database.Update_MarbeteDescargado(marbete.Marbete);

                                                        transaction.Commit();

                                                        marbeteDescargado = true;

                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    transaction.Rollback();
                                                    msjError = "Ocurrió un problema al intentar obtener la información del Marbete " + marbete.Marbete;
                                                    break;
                                                }
                                            }
                                        }

                                        if (!marbeteDescargado)
                                        {
                                            transaction.Rollback();
                                            msjError = "Ocurrió un problema al intentar descargar la información del Marbete " + marbete.Marbete;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        msjError = "El Marbete " + marbete.Marbete + " no contiene información, favor de verificarlo";
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(msjErrorCerrado))
                                    msjErrorCerrado = marbete.Marbete.ToString();
                                else
                                {
                                    msjErrorCerrado += ", " + marbete.Marbete.ToString();
                                }
                            }
                        }
                    }

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
                }
                catch (Exception ex)
                {
                    msjError = ex.Message;
                }
                finally
                {
                    if (cnnDB != null && cnnDB.State == System.Data.ConnectionState.Open)
                        cnnDB.Close();

                    UserDialogs.Instance.HideHud();
                }

                if (!String.IsNullOrEmpty(msjErrorCerrado))
                {
                    if (String.IsNullOrEmpty(msjError))
                        msjError = "Los siguientes Marbetes no se descargaron debido a que ya se encontraban cerrados en la Base de datos del Inventario: " + msjErrorCerrado;
                    else
                        msjError += " Los siguientes Marbetes no se descargaron debido a que ya se encontraban cerrados en la Base de datos del Inventario: " + msjErrorCerrado;
                }

                if (String.IsNullOrEmpty(msjError))
                {
                    await Shell.Current.DisplayAlert("", "La descarga de los Marbetes se realizó con éxito", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("", "No fue posible realizar la descarga completamente. " + msjError, "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideHud();
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
            finally
            {
                PermiteDescargar = true;
                Carga_Marbetes();
            }
        }

        [RelayCommand]
        async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
