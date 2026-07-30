using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class ItemMarbeteVM : ObservableObject
    {
        [ObservableProperty]
        string marbete;

        [ObservableProperty]
        string estatus;

        [ObservableProperty]
        bool visible;

        [RelayCommand]
        async void Abrir()
        {
            var result = await Shell.Current.DisplayAlert("", "¿Desea abrir el Marbete seleccionado", "OK", "Cancelar");
            if (result)
            {
                await Shell.Current.DisplayAlert("", "El Marbete ha sido Abierto", "OK");
            }
        }

        [RelayCommand]
        async void Descargar()
        {   
                await Shell.Current.DisplayAlert("", "El Marbete se ha descargado con exito", "OK");            
        }
    }
}
