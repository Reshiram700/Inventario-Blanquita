using Blanquita_Inventarios.AppMAUI.Service;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class MainVM : ObservableObject
    {
        private ApiService apiService;
        private readonly DatabaseService _database;

        [ObservableProperty]
        string usuario;

        [ObservableProperty]
        string catalogoActual;

        [ObservableProperty]
        bool visibleBorrar;

        [ObservableProperty]
        string colorCatalogos;

        [ObservableProperty]
        string nombrePDA;

        // Nuevas propiedades para el porcentaje de descarga
        [ObservableProperty]
        bool isDownloading;

        [ObservableProperty]
        double downloadProgress;

        [ObservableProperty]
        string downloadProgressText;

        public MainVM()
        {
            apiService = new ApiService();
            _database = new DatabaseService(FileSystem.AppDataDirectory);

            Usuario = string.Empty;
            CatalogoActual = "--";
            VisibleBorrar = false;

            // Inicialmente naranja (sin catálogos descargados)
            ColorCatalogos = "#ec851a";

            // Propiedades del progreso
            IsDownloading = false;
            DownloadProgress = 0;
            DownloadProgressText = "";

            if (Preferences.ContainsKey("User_Nombre"))
                Usuario = Preferences.Get("User_Nombre", "No especificado");

            if (Preferences.ContainsKey("User_IdPerfil"))
            {
                int idPerfil = Preferences.Get("User_IdPerfil", 0);
                VisibleBorrar = idPerfil == 4;
            }

            CompruebaDBSqlite();

            NombrePDA = ObtenerNombreBluetooth();
        }

        private async void CompruebaDBSqlite()
        {
            await _database.InitAsync();

            var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
            if (parametrizacion.Count > 0)
            {
                var parametros = parametrizacion.FirstOrDefault(s => s.IdParametrizacion == 1);
                if (parametros != null)
                {
                    Preferences.Set("IdConfiguracion", parametros.IdConfiguracion);
                    Preferences.Set("NombrePDA", parametros.NombrePDA);
                    Preferences.Set("Almacen", parametros.Almacen);
                    Preferences.Set("Capturados", parametros.Capturados);
                    Preferences.Set("Descargados", parametros.Descargados);
                }
            }

            await VerificarCatalogosDescargados();
        }


        private async Task VerificarCatalogosDescargados()
        {
            var listadoArticulos = await _database.Obtener_SAP_Articulos_Async();

            // Si NO hay artículos, no hay catálogo descargado
            if (listadoArticulos.Count == 0)
            {
                ColorCatalogos = "#ec851a";
                CatalogoActual = "--";
                return;
            }

            // Hay catálogo descargado
            ColorCatalogos = "#307f0d";

            // 1️⃣ PRIORIDAD: Preferences (nombre correcto del catálogo)
            if (Preferences.ContainsKey("NombreCatalogo"))
            {
                CatalogoActual = Preferences.Get("NombreCatalogo", "--");
                return;
            }

            // 2️⃣ RESPALDO: SQLite (NombreCatalogo, NO NombrePDA)
            var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
            var parametros = parametrizacion.FirstOrDefault(p => p.IdParametrizacion == 1);

            if (parametros != null && !string.IsNullOrEmpty(parametros.NombreCatalogo))
            {
                CatalogoActual = parametros.NombreCatalogo;
            }
            else
            {
                CatalogoActual = "--";
            }
        }

        private string ObtenerNombreBluetooth()
        {
#if ANDROID
            try
            {
                var adapter = Android.Bluetooth.BluetoothAdapter.DefaultAdapter;
                if (adapter != null)
                {
                    return adapter.Name;
                }
                return "Bluetooth no disponible";
            }
            catch
            {
                return "No se pudo obtener el nombre";
            }
#else
    return "No disponible en esta plataforma";
#endif
        }



        [RelayCommand]
        async void Salir()
        {
            Preferences.Remove("User_Nombre");
            Preferences.Remove("User_ID");
            Preferences.Remove("User_IdPerfil");

            await Shell.Current.GoToAsync("..");
        }


        [RelayCommand]
        async void Parametrizacion()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(ParametrizacionPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Configuracion()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(ConfiguracionPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Catalogos()
        {
            if (Preferences.ContainsKey("IdConfiguracion"))
            {
                int idConfiguracion = Preferences.Get("IdConfiguracion", 0);
                if (idConfiguracion > 0)
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading("Espere un momento...");

                        var urlApi = App.Current.Resources["UrlAPI"].ToString();
                        var response = await apiService.Get<Configuraciones>(urlApi, "/api", "/GetConfiguracion", idConfiguracion);

                        if (response.ExecutionOK)
                        {
                            Configuraciones configuracion = response.Data;

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

                            bool hayDatos = false;

                            //Revisamos si hay datos del SAP en la BD Sqlite del dispotivo
                            var listadoArticulos = await _database.Obtener_SAP_Articulos_Async();
                            if (listadoArticulos.Count > 0)
                            {
                                UserDialogs.Instance.HideHud();
                                hayDatos = true;
                                //Si hay datos preguntamos si lo desea eliminar
                                var result = await Shell.Current.DisplayAlert("", "Actualmente el dispositivo tiene informacion ¿Desea limpiar los datos?", "Si", "Cancelar");
                                if (result)
                                {
                                    //Si dice si solicitamos un codigo de autorizacion
                                    var resultCodigo = await Shell.Current.DisplayPromptAsync("", "Ingrese el Código de autorización");
                                    if (!String.IsNullOrEmpty(resultCodigo))
                                    {
                                        int IdUsuario = 0;
                                        if (Preferences.ContainsKey("User_ID"))
                                            IdUsuario = Preferences.Get("User_ID", 0);

                                        UseCodigo usar = new UseCodigo
                                        {
                                            IdConfiguracion = idConfiguracion,
                                            Codigo = resultCodigo,
                                            IdAccion = 1,
                                            IdUsuario = IdUsuario
                                        };

                                        var responseUsarCodigo = await apiService.PostObj<int>(urlApi, "/api", "/UsarCodigo", usar);
                                        if (responseUsarCodigo.ExecutionOK)
                                        {
                                            // Poner el botón en naranja antes de empezar la descarga
                                            ColorCatalogos = "#ec851a";
                                            CatalogoActual = "--";


                                            //Si el codigo es aceptado eliminamos los datos que hay en la BD Sqlite del dispositivo
                                            await _database.Eliminar_SAP_Articulos_Async();
                                            await _database.Eliminar_SAP_Barcodes_Async();

                                            await _database.Eliminar_SI_Zonas_Async();
                                            await _database.Eliminar_SI_Marbetes_Async();
                                            await _database.Eliminar_SI_MarbetesArticulos_Async();

                                            hayDatos = false;
                                        }
                                        else
                                        {
                                            await Shell.Current.DisplayAlert("", responseUsarCodigo.Message, "OK");
                                        }
                                    }
                                }
                            }

                            UserDialogs.Instance.HideHud();
                            //Descargamos los datos de la BD Sql del Inventario
                            if (!hayDatos)
                            {
                                // Iniciar proceso de descarga con porcentaje
                                IsDownloading = true;
                                DownloadProgress = 0;
                                DownloadProgressText = "Preparando descarga...";

                                // Contar total de registros primero
                                int totalRecords = await ContarTotalRegistros(cnnDB);
                                int currentRecords = 0;

                                // Quitamos UserDialogs.Instance.ShowLoading("Descargando...");

                                //ARTICULOS
                                DownloadProgressText = "Descargando artículos...";

                                var listaArticulos = new List<SAP_Articulos_Sqlite>();

                                string query = "SELECT WhsCode, ItemCode, ItemName, Price FROM SAP_Articulos";
                                using (SqlCommand command = new SqlCommand(query, cnnDB))
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        listaArticulos.Add(new SAP_Articulos_Sqlite
                                        {
                                            WshCode = reader.GetString(0),
                                            ItemCode = reader.GetString(1),
                                            ItemName = reader.GetString(2),
                                            Price = reader.GetDecimal(3)
                                        });

                                        currentRecords++;
                                        DownloadProgress = (double)currentRecords / totalRecords;
                                    }
                                }

                                await _database.InsertarArticulosBulkAsync(listaArticulos);

                                //BARCODES
                                DownloadProgressText = "Descargando códigos de barras...";

                                var listaBarcodes = new List<SAP_Barcodes_Sqlite>();

                                query = "SELECT ItemCode, BcdCode, UomCode, BaseQty FROM SAP_Barcodes";
                                using (SqlCommand command = new SqlCommand(query, cnnDB))
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        listaBarcodes.Add(new SAP_Barcodes_Sqlite
                                        {
                                            ItemCode = reader.GetString(0),
                                            BcdCode = reader.GetString(1),
                                            UomCode = reader.GetString(2),
                                            BaseQty = reader.GetDecimal(3)
                                        });

                                        currentRecords++;
                                        DownloadProgress = (double)currentRecords / totalRecords;
                                    }
                                }

                                await _database.InsertarBarcodesBulkAsync(listaBarcodes);


                                //Zonas                                
                                DownloadProgressText = "Descargando zonas...";

                                var listaZonas = new List<SI_Zonas_Sqlite>();

                                query = "SELECT IdZona, Zona, MarbeteInicial, MarbeteFinal FROM Zonas WHERE Activo = 1";
                                using (SqlCommand command = new SqlCommand(query, cnnDB))
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        listaZonas.Add(new SI_Zonas_Sqlite
                                        {
                                            IdZona = reader.GetInt32(0),
                                            Zona = reader.GetString(1),
                                            MarbeteInicial = reader.GetInt32(2),
                                            MarbeteFinal = reader.GetInt32(3)
                                        });

                                        currentRecords++;
                                        DownloadProgress = (double)currentRecords / totalRecords;
                                    }
                                }

                                await _database.InsertarZonasBulkAsync(listaZonas);


                                //Marbetes
                                DownloadProgressText = "Descargando marbetes...";

                                var listaMarbetes = new List<SI_Marbetes_Sqlite>();

                                query = "SELECT IdMarbete, IdZona, Marbete, Estatus FROM dbo.Marbetes";
                                using (SqlCommand command = new SqlCommand(query, cnnDB))
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        listaMarbetes.Add(new SI_Marbetes_Sqlite
                                        {
                                            IdMarbete = reader.GetInt32(0),
                                            IdZona = reader.GetInt32(1),
                                            Marbete = reader.GetInt32(2),
                                            Estatus = reader.GetInt32(3)
                                        });

                                        currentRecords++;
                                        DownloadProgress = (double)currentRecords / totalRecords;
                                    }
                                }

                                await _database.InsertarMarbetesBulkAsync(listaMarbetes);


                                Preferences.Set("Capturados", 0);
                                Preferences.Set("Descargados", 0);

                                var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
                                if (parametrizacion.Count > 0)
                                {
                                    var parametros = parametrizacion.Where(s => s.IdParametrizacion == 1).FirstOrDefault();
                                    if (parametros != null)
                                    {
                                        parametros.Capturados = 0;
                                        parametros.Descargados = 0;
                                        await _database.Update_Parametrizacion(parametros);
                                    }
                                }

                                // Finalizar descarga
                                IsDownloading = false;
                                DownloadProgress = 1.0;
                                DownloadProgressText = "¡Descarga completada!";

                                // Poner el botón en verde (catálogos descargados)
                                ColorCatalogos = "#307f0d";
                                CatalogoActual = Preferences.Get("NombreCatalogo", "--");


                                await Shell.Current.DisplayAlert("", "La información se ha descargado con exito", "OK");
                            }

                            if (cnnDB.State == System.Data.ConnectionState.Open)
                                cnnDB.Close();
                        }
                        else
                        {
                            UserDialogs.Instance.HideHud();
                            await Shell.Current.DisplayAlert("", response.Message, "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        IsDownloading = false;
                        UserDialogs.Instance.HideHud();
                        await Shell.Current.DisplayAlert("", ex.Message, "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("", "No se ha seleccionado un Inventario, revise su Configuración", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("", "No se ha seleccionado un Inventario, revise su Configuración", "OK");
            }
        }

        private async Task<int> ContarTotalRegistros(SqlConnection cnnDB)
        {
            int total = 0;

            try
            {
                // Contar ARTICULOS
                string countQuery = "SELECT COUNT(*) FROM SAP_Articulos";
                using (SqlCommand command = new SqlCommand(countQuery, cnnDB))
                {
                    total += Convert.ToInt32(command.ExecuteScalar());
                }

                // Contar BARCODES
                countQuery = "SELECT COUNT(*) FROM SAP_Barcodes";
                using (SqlCommand command = new SqlCommand(countQuery, cnnDB))
                {
                    total += Convert.ToInt32(command.ExecuteScalar());
                }

                // Contar ZONAS
                countQuery = "SELECT COUNT(*) FROM Zonas WHERE Activo = 1";
                using (SqlCommand command = new SqlCommand(countQuery, cnnDB))
                {
                    total += Convert.ToInt32(command.ExecuteScalar());
                }

                // Contar MARBETES
                countQuery = "SELECT COUNT(*) FROM dbo.Marbetes";
                using (SqlCommand command = new SqlCommand(countQuery, cnnDB))
                {
                    total += Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception)
            {
                // Si hay error al contar, usar un valor por defecto
                total = 1;
            }

            return total > 0 ? total : 1; // Evitar división por cero
        }

        [RelayCommand]
        async void Capturar()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(CapturarPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Capturados()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(CapturadosPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Pendientes()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(PendientesPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Ver()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(VerPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Borrar()
        {
            var result = await Shell.Current.DisplayAlert("", "¿Esta seguro de limpiar toda la información del dispositivo?", "OK", "Cancelar");
            if (result)
            {
                try
                {
                    // Poner el botón en naranja (sin catálogos)
                    ColorCatalogos = "#ec851a";
                    CatalogoActual = "--";


                    //Si el codigo es aceptado eliminamos los datos que hay en la BD Sqlite del dispositivo
                    await _database.Eliminar_SAP_Articulos_Async();
                    await _database.Eliminar_SAP_Barcodes_Async();
                    await _database.Eliminar_SI_Zonas_Async();
                    await _database.Eliminar_SI_Marbetes_Async();
                    await _database.Eliminar_SI_MarbetesArticulos_Async();

                    await Shell.Current.DisplayAlert("", "La informacion se limpio con exito", "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("", ex.Message, "OK");
                }
            }
        }
    }
}