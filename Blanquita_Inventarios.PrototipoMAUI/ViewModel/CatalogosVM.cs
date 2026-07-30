using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class CatalogosVM : ObservableObject
    {
        public CatalogosVM()
        {

        }

        [RelayCommand]
        async void Marbetes()
        {
            try
            {

                await Shell.Current.DisplayAlert("", "Se procedera a traer el Catálogo de Marbetes", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void Articulos()
        {
            try
            {

                await Shell.Current.DisplayAlert("", "Se procedera a traer el Catálogo de Artículos", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
