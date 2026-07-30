using Blanquita_Inventarios.AppMAUI.Generics;
using Blanquita_Inventarios.AppMAUI.Service;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
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
    public partial class CapturarVM : ObservableObject, IQueryAttributable
    {
        private ApiService apiService;

        private readonly DatabaseService _database;

        [RelayCommand]
        void FocusView(View view)
        {
            view?.Focus();
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Barcode_Datos"))
            {
                DatosTomados = (DatosBarcode)query["Barcode_Datos"];

                if (DatosTomados.Tipo == 1)
                {
                    Marbete = DatosTomados.Marbete.ToString();
                }
                else
                {
                    EsCapturable = true;
                    CodigoProducto = DatosTomados.Codigo;
                    ItemCode = DatosTomados.ItemCode;
                    Descripcion = DatosTomados.Descripcion;
                    Uom = DatosTomados.Uom;
                    EsCapturable = false;
                }
            }
            else
            {
                DatosTomados = new DatosBarcode();
            }
        }

        [ObservableProperty]
        string almacen;

        [ObservableProperty]
        string marbete;

        [ObservableProperty]
        string codigoProducto;

        [ObservableProperty]
        string itemCode;

        [ObservableProperty]
        string descripcion;

        [ObservableProperty]
        string uom;

        [ObservableProperty]
        string cantidad;

        [ObservableProperty]
        string nombrePda;

        [ObservableProperty]
        bool esCapturable;

        [ObservableProperty]
        DatosBarcode datosTomados;

        [ObservableProperty]
        bool enabledMarbete;

        [ObservableProperty]
        bool btnEnabled;

        [ObservableProperty]
        View controlMarbete;

        [ObservableProperty]
        View controlBarcode;

        [ObservableProperty]
        bool permiteSalir;

        [ObservableProperty]
        bool habilitarCaptura;

        [ObservableProperty]
        bool enabledGrabar;

        public CapturarVM()
        {
            apiService = new ApiService();
            _database = new DatabaseService(FileSystem.AppDataDirectory);

            EsCapturable = false;
            EnabledMarbete = false;
            BtnEnabled = true;
            PermiteSalir = true;
            HabilitarCaptura = false;

            EnabledGrabar = true;

            NombrePda = ObtenerNombreBluetooth();
                        
            if (String.IsNullOrEmpty(NombrePda))
            {
                EnabledGrabar = false;
                Shell.Current.DisplayAlert("", "No es posible capturar información ya que no se ha asignado un Nombre a la PDA, regrese y revise la parametrización", "OK");                
                return;
            }

            if (Preferences.ContainsKey("Almacen"))
                Almacen = Preferences.Get("Almacen", "");
        }

        public void Limpiar()
        {
            Marbete = string.Empty;
            EnabledMarbete = false;
            BtnEnabled = true;
            PermiteSalir = true;
            HabilitarCaptura = false;

            EsCapturable = true;
            CodigoProducto = string.Empty;
            ItemCode = string.Empty;
            Descripcion = string.Empty;
            Uom = string.Empty;
            Cantidad = string.Empty;
            EsCapturable = false;
        }

        [RelayCommand]
        async void TomarMarbete()
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Barcode_Tipo", 1 }
                };
                await Shell.Current.GoToAsync(nameof(BarcodePage), navigationParameter);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void TomarCodigo()
        {
            try
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Barcode_Tipo", 2 }
                };
                await Shell.Current.GoToAsync(nameof(BarcodePage), navigationParameter);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }


        [RelayCommand]
        async void BuscarMarbete(View control)
        {
            ValidarMarbete(control);
        }

        async void ValidarMarbete(View control)
        {
            try
            {
                if (!String.IsNullOrEmpty(Marbete))
                {
                    int intMarbete = 0;
                    if (int.TryParse(Marbete, out intMarbete))
                    {
                        if (Preferences.ContainsKey("IdConfiguracion"))
                        {
                            int idConfiguracion = Preferences.Get("IdConfiguracion", 0);
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

                                    SqlCommand cmdMarbete = new SqlCommand(Querys_SAP_SQL.Get_SQL_MarbeteByNoMarbete(), cnnDB);
                                    cmdMarbete.Parameters.AddWithValue("@Marbete", intMarbete);

                                    DataTable dtMarbete = new DataTable();
                                    SqlDataAdapter daMarbete = new SqlDataAdapter(cmdMarbete);
                                    daMarbete.Fill(dtMarbete);

                                    // CORRECCIÓN AQUÍ: Agregar validación cuando NO existe
                                    if (dtMarbete.Rows.Count > 0)
                                    {
                                        int estatus = int.Parse(dtMarbete.Rows[0][2].ToString());
                                        if (estatus != 3)
                                        {
                                            DBResponse<SI_Marbetes_Sqlite> response = await _database.Get_Marbete(intMarbete);
                                            if (response.ExecutionOK)
                                            {
                                                if (response.Data != null)
                                                {
                                                    if (response.Data.Estatus == 1)
                                                    {
                                                        FocusView(control);
                                                        HabilitarCaptura = true;

                                                        CodigoProducto = string.Empty;
                                                        ItemCode = string.Empty;
                                                        Descripcion = string.Empty;
                                                        Uom = string.Empty;
                                                        Cantidad = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        Marbete = string.Empty;
                                                        FocusView(ControlMarbete);
                                                        await Shell.Current.DisplayAlert("", "El Marbete que intenta capturar se encuentra cerrado en el dispositivo.", "OK");
                                                    }
                                                }
                                                else
                                                {
                                                    Marbete = string.Empty;
                                                    FocusView(ControlMarbete);
                                                    await Shell.Current.DisplayAlert("", "No se encontro el Marbete ingresado.", "OK");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FocusView(ControlMarbete);
                                            await Shell.Current.DisplayAlert("", "El Marbete se encuentra cerrado en el Servidor del Inventario.", "OK");
                                        }
                                    }
                                    else
                                    {
                                        // CORRECCIÓN: Mensaje cuando el marbete NO EXISTE en SQL Server
                                        Marbete = string.Empty;
                                        FocusView(ControlMarbete);
                                        await Shell.Current.DisplayAlert("", $"El marbete {intMarbete} no existe en el sistema.", "OK");
                                    }
                                }
                                else
                                {
                                    FocusView(ControlMarbete);
                                    await Shell.Current.DisplayAlert("", responseConfiguracion.Message, "OK");
                                }
                            }
                            else
                            {
                                FocusView(ControlMarbete);
                                await Shell.Current.DisplayAlert("", "No se ha seleccionado un Inventario, revise su Configuración", "OK");
                            }
                        }
                        else
                        {
                            FocusView(ControlMarbete);
                            await Shell.Current.DisplayAlert("", "No se ha seleccionado un Inventario, revise su Configuración", "OK");
                        }
                    }
                    else
                    {
                        FocusView(ControlMarbete);
                        await Shell.Current.DisplayAlert("", "Asegurese de ingresar un número de Marbete", "OK");
                    }
                }
                else
                {
                    FocusView(ControlMarbete);
                    await Shell.Current.DisplayAlert("", "Ingrese el número de Marbete", "OK");
                }
            }
            catch (Exception ex)
            {
                FocusView(ControlMarbete);
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }


        [RelayCommand]
        async void BuscarCodigo(View control)
        {
            ValidaBarcode(control);
        }

        [RelayCommand]
        async void BuscarCodigoText(View control)
        {
            ValidaBarcode(control);
        }

        async void ValidaBarcode(View control)
        {
            try
            {
                if (!String.IsNullOrEmpty(CodigoProducto))
                {
                    DatosTomados = await _database.Buscar_DatosScanBarcodeProducto_Async(CodigoProducto);

                    if (String.IsNullOrEmpty(DatosTomados.Codigo))
                    {
                        CodigoProducto = string.Empty; ;
                        ItemCode = string.Empty; ;
                        Descripcion = string.Empty; ;
                        Uom = string.Empty;

                        FocusView(ControlBarcode);
                        await Shell.Current.DisplayAlert("", "No se encontro información sobre el Codigo " + CodigoProducto, "OK");
                    }
                    else
                    {
                        EsCapturable = true;
                        CodigoProducto = DatosTomados.Codigo;
                        ItemCode = DatosTomados.ItemCode;
                        Descripcion = DatosTomados.Descripcion;
                        Uom = DatosTomados.Uom;
                        EsCapturable = false;

                        FocusView(control);
                    }
                }
                else
                {
                    FocusView(ControlBarcode);
                }

            }
            catch (Exception ex)
            {
                FocusView(ControlBarcode);
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void BuscarItem()
        {
            try
            {
                if (!String.IsNullOrEmpty(ItemCode))
                {
                    DatosBarcode datosItem = await _database.Buscar_DatosItemCodeProducto_Async(ItemCode);

                    if (datosItem != null)
                    {
                        CodigoProducto = datosItem.Codigo;
                        ItemCode = datosItem.ItemCode;
                        Descripcion = datosItem.Descripcion;
                        Uom = datosItem.Uom;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("", "No se encontro información del Producto", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("", "Ingrese el ItemCode del Producto", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }


        [RelayCommand]
        async void EjecutaGrabar()
        {
            Grabar(ControlBarcode);
        }


        [RelayCommand]
        async void Grabar(View control)
        {
            try
            {
                if (!String.IsNullOrEmpty(Marbete) && !String.IsNullOrEmpty(CodigoProducto) && !String.IsNullOrEmpty(ItemCode) && !String.IsNullOrEmpty(Descripcion) && !String.IsNullOrEmpty(Uom) && !String.IsNullOrEmpty(Cantidad))
                {
                    bool paseRegistro = true;

                    int intMarbete = 0;
                    if (!int.TryParse(Marbete, out intMarbete))
                    {
                        await Shell.Current.DisplayAlert("", "Asegúrese de ingresar un número de Marbete", "OK");
                        return;
                    }

                    decimal intCantidad = 0;
                    if (!decimal.TryParse(Cantidad, out intCantidad))
                    {
                        await Shell.Current.DisplayAlert("", "Asegúrese de ingresar una Cantidad numérica o la cantidad que ingreso es demasiado grande, revíselo.", "OK");
                        return;
                    }

                    // Máximo 6 dígitos (999999)
                    if (intCantidad > 999999)
                    {
                        await Shell.Current.DisplayAlert("",
                            "La cantidad máxima permitida es 999999 (6 dígitos).",
                            "OK");
                        return;
                    }

                    if (intCantidad >= 1000)
                    {
                        paseRegistro = false;

                        while (!paseRegistro)
                        {
                            string numRandom = CreateNumber();
                            var resultCodigo = await Shell.Current.DisplayPromptAsync("", "¿Esta seguro de capturar la cantidad de " + intCantidad.ToString() + "?, si es así ingrese el Código " + numRandom + " para confirmar la acción.");
                            if (resultCodigo == null)
                            {
                                break;
                            }
                            else
                            {
                                if (resultCodigo == numRandom)
                                    paseRegistro = true;
                                else
                                {
                                    await Shell.Current.DisplayAlert("", "El Codigo que ingreso es incorrecto, intente nuevamente", "OK");
                                }
                            }
                        }
                    }

                    if (paseRegistro)
                    {
                        DatosBarcode datosRegistrar = new DatosBarcode
                        {
                            Marbete = intMarbete,
                            Codigo = CodigoProducto,
                            ItemCode = ItemCode,
                            Descripcion = Descripcion,
                            Uom = Uom,
                            BaseQty = DatosTomados.BaseQty,
                            Cantidad = intCantidad,
                            Precio = DatosTomados.Precio,
                            Costo = (intCantidad * DatosTomados.BaseQty) * DatosTomados.Precio,
                            NombrePDA = NombrePda
                        };

                        var response = await _database.Insert_ArticuloMarbete_Async(datosRegistrar);
                        if (response.ExecutionOK)
                        {
                            var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
                            if (parametrizacion.Count > 0)
                            {
                                var parametros = parametrizacion.Where(s => s.IdParametrizacion == 1).FirstOrDefault();
                                if (parametros != null)
                                {
                                    int marbetesCapturados = 0;
                                    var responseMD = await _database.Get_MarbetesCapturados();
                                    if (responseMD.ExecutionOK)
                                        marbetesCapturados = responseMD.Data.Count();

                                    parametros.Capturados = marbetesCapturados;
                                    await _database.Update_Parametrizacion(parametros);
                                    Preferences.Set("Capturados", parametros.Capturados);
                                }
                            }

                            PermiteSalir = false;

                            EsCapturable = true;
                            CodigoProducto = string.Empty;
                            ItemCode = string.Empty;
                            Descripcion = string.Empty;
                            Uom = string.Empty;
                            Cantidad = string.Empty;
                            EsCapturable = false;
                            EnabledMarbete = true;
                            BtnEnabled = false;
                            FocusView(control);
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("", response.Message, "OK");
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("", "Proporcione la información necesaria para la captura del Producto", "OK");
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        private string ObtenerNombreBluetooth()
        {
       #if ANDROID
            try
            {
                var adapter = BluetoothAdapter.DefaultAdapter;

                if (adapter == null)
                    return string.Empty;

                var nombre = adapter.Name;

                if (string.IsNullOrWhiteSpace(nombre))
                    return string.Empty;

                return nombre;
            }
            catch
            {
                return string.Empty;
            }
        #else
    return string.Empty;
        #endif
        }


        public static string CreateNumber()
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            int length = 3;
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        [RelayCommand]
        async void Cancelar(View control)
        {
            try
            {
                CodigoProducto = string.Empty;
                ItemCode = string.Empty;
                Descripcion = string.Empty;
                Uom = string.Empty;
                Cantidad = string.Empty;

                FocusView(control);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Cerrar(View control)
        {
            try
            {
                if (String.IsNullOrEmpty(Marbete))
                {
                    await Shell.Current.DisplayAlert("", "Ingrese un Marbete valido", "OK");
                    return;
                }

                var resulto = await Shell.Current.DisplayAlert("",
                    "¿Esta seguro de cerrar el Marbete?",
                    "OK",
                    "Cancelar");

                if (!resulto)
                    return;

                int intMarbete = 0;
                if (!int.TryParse(Marbete, out intMarbete))
                {
                    await Shell.Current.DisplayAlert("",
                        "Asegurese de ingresar un número de Marbete",
                        "OK");
                    return;
                }

                // ✅ VALIDACIÓN NUEVA
                int totalArticulos = await _database.Count_ArticulosByMarbete(intMarbete);

                if (totalArticulos == 0)
                {
                    await Shell.Current.DisplayAlert("",
                        "No es posible cerrar el Marbete porque no contiene artículos capturados.",
                        "OK");
                    return;
                }

                // 🔒 Cierre normal
                var response = await _database.Cerrar_Marbete(intMarbete, NombrePda);

                if (response.ExecutionOK)
                {
                    Limpiar();
                    FocusView(control);
                }
                else
                {
                    await Shell.Current.DisplayAlert("", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }


        [RelayCommand]
        async void Regresar()
        {
            if (PermiteSalir)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("", "El Marbete no se ha cerrado, para regresar a la pantalla principal es necesario que cierre primeramente el Marbete", "OK");
            }

        }
    }
}
