using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class MainVM : ObservableObject
    {
        public MainVM()
        {

        }

        [RelayCommand]
        async void Ingresar()
        {
            try
            {

                string msjBienvenida = "Bienvenido";
                await Shell.Current.DisplayAlert("", msjBienvenida, "OK");
                await Shell.Current.GoToAsync(nameof(MainPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
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
            try
            {

                await Shell.Current.GoToAsync(nameof(CatalogosPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
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
        async void Borrar()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(BorrarPage));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

    }
}
