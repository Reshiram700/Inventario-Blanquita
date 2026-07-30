using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class BorrarVM : ObservableObject
    {
        public BorrarVM()
        {
            
        }

        [RelayCommand]
        async void Aplicar()
        {
            var result = await Shell.Current.DisplayAlert("", "¿Esta seguro de borrar toda la información?", "OK", "Cancelar");
            if (result)
            {
                await Shell.Current.DisplayAlert("", "La información ha sido borrada correctamente", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
