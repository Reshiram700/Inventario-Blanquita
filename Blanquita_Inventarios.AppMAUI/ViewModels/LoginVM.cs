using Blanquita_Inventarios.AppMAUI.Service;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class LoginVM : ObservableObject
    {
        private ApiService apiService;

        [ObservableProperty]
        string usuario;
        [ObservableProperty]
        string password;

        public LoginVM()
        {
            apiService = new ApiService();
            Usuario = String.Empty;
            Password = String.Empty;

            Preferences.Clear();
        }

        [RelayCommand]
        async void Ingresar()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Espere un momento...");

                if (String.IsNullOrEmpty(Usuario.Trim()) || String.IsNullOrEmpty(Password.Trim()))
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "Ingrese la información solicitada", "OK");                    
                    return;
                }

                if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", "Revise su conexión a internet", "OK");                    
                    return;
                }

                UserLogin user = new UserLogin
                {
                    Usuario = Usuario.Trim(),
                    Password = Password.Trim()
                };
                var urlApi = App.Current.Resources["UrlAPI"].ToString();
                var response = await apiService.PostObj<UsuarioSesion>(urlApi, "/api", "/Login", user);

                if (response.ExecutionOK)
                {
                    Usuario = string.Empty;
                    Password = string.Empty;

                    string msjBienvenida = "Bienvenido " + response.Data.Nombre;

                    Preferences.Set("User_ID", response.Data.Id);
                    Preferences.Set("User_Nombre", response.Data.Nombre);
                    Preferences.Set("User_IdPerfil", response.Data.IdPerfil);

                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", msjBienvenida, "OK");
                    await Shell.Current.GoToAsync(nameof(MainPage));
                }
                else
                {
                    UserDialogs.Instance.HideHud();
                    await Shell.Current.DisplayAlert("", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideHud();
                await Shell.Current.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
