using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class CapturarVM : ObservableObject
    {
        public CapturarVM()
        {

        }

        [RelayCommand]
        async void Grabar()
        {
            try
            {

                await Shell.Current.DisplayAlert("", "La información ha sido grabada", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Cancelar()
        {
            try
            {
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Cerrar()
        {
            try
            {

                await Shell.Current.DisplayAlert("", "El Marbete a sido cerrado correctamente", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
