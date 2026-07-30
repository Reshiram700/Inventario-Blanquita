using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class ParametrizacionVM : ObservableObject
    {
        public ParametrizacionVM()
        {
            
        }

        [RelayCommand]
        async void Actualizar()
        {
            try
            {

                await Shell.Current.DisplayAlert("", "Los datos han sido actualizados", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
