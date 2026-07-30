using Blanquita_Inventarios.AppMAUI.Service;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class ConfiguracionVM : ObservableObject
    {
        private ApiService apiService;
        private readonly DatabaseService _database;

        [ObservableProperty]
        public List<ControlDDL> listaInventarios;

        [ObservableProperty]
        ControlDDL inventarioSeleccionado;

        [ObservableProperty]
        int indexInventarioSeleccionado;

        public ConfiguracionVM()
        {
            apiService = new ApiService();
            _database = new DatabaseService(FileSystem.AppDataDirectory);

            CargaInventarios();
        }

        private void ObtenerInventario()
        {
            int idConfiguracion = 0;

            if (Preferences.ContainsKey("IdConfiguracion"))
            {
                idConfiguracion = Preferences.Get("IdConfiguracion", 0);
            }

            if (ListaInventarios != null && ListaInventarios.Count > 0)
            {
                var item = ListaInventarios.Where(w => w.Valor == idConfiguracion).FirstOrDefault();
                if (item != null)
                {
                    InventarioSeleccionado = item;
                }
            }
        }

        private async void CargaInventarios()
        {
            try
            {
                var urlApi = App.Current.Resources["UrlAPI"].ToString();
                var response = await apiService.GetData<List<ControlDDL>>(urlApi, "/api", "/GetInventarios");

                if (response.ExecutionOK)
                {
                    ListaInventarios = response.Data;
                    ObtenerInventario();
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
        async void SeleccionItemPicker()
        {

        }

        [RelayCommand]
        async void Seleccionar()
        {
            if (InventarioSeleccionado.Valor > 0)
            {
                Configuraciones configuracion = new Configuraciones();
                var urlApi = App.Current.Resources["UrlAPI"].ToString();
                var response = await apiService.Get<Configuraciones>(urlApi, "/api", "/GetConfiguracion", InventarioSeleccionado.Valor);
                if (response.ExecutionOK)
                {
                    configuracion = response.Data;

                }
                else
                {
                    await Shell.Current.DisplayAlert("", "No fue posible obtener la información del Inventario seleccionado", "OK");
                    return;
                }

                var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
                if (parametrizacion.Count > 0)
                {
                    var parametros = parametrizacion.Where(s => s.IdParametrizacion == 1).FirstOrDefault();
                    if (parametros != null)
                    {
                        parametros.IdConfiguracion = InventarioSeleccionado.Valor;
                        parametros.Almacen = configuracion.Almacen;
                        parametros.NombreCatalogo = InventarioSeleccionado.Texto;

                        await _database.Update_Parametrizacion(parametros);

                    }
                }
                else
                {
                    await _database.Guardar_SI_Parametrizacion_Async(new SI_Parametrizacion_Sqlite()
                    {
                        IdParametrizacion = 1,
                        NombreCatalogo = InventarioSeleccionado.Texto,
                        IdConfiguracion = InventarioSeleccionado.Valor,
                        Almacen = configuracion.Almacen
                    });

                }

                Preferences.Set("IdConfiguracion", InventarioSeleccionado.Valor);
                Preferences.Set("Almacen", configuracion.Almacen);
                Preferences.Set("NombreCatalogo", InventarioSeleccionado.Texto);


                await Shell.Current.DisplayAlert("", "Se selecciono el Inventario correctamente", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("", "Seleccione un Inventario", "OK");
            }
        }

        [RelayCommand]
        async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
