using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using System.Security.Cryptography;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class LoginVM : ObservableObject    
    {
        [ObservableProperty]
        string usuario;
        [ObservableProperty]
        string password;

        public LoginVM()
        {
            Usuario = String.Empty;
            Password = String.Empty;
        }


        [RelayCommand]
        async void Ingresar()
        {
            try
            {

                UserDialogs.Instance.ShowLoading("Procesando");
                await Task.Delay(3000);
                UserDialogs.Instance.HideHud();

                string msjBienvenida = "Bienvenido";
                    await Shell.Current.DisplayAlert("", msjBienvenida, "OK");
                    await Shell.Current.GoToAsync(nameof(MainPage));
               
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
