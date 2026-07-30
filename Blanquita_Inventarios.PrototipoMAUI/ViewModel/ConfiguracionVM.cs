using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class ConfiguracionVM : ObservableObject
    {
        public ConfiguracionVM()
        {

        }

        [RelayCommand]
        async void Seleccionar()
        {
            try
            {

                await Shell.Current.DisplayAlert("", "Se selecciono el Inventario correctamente", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
