using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

#if ANDROID
using Android.Bluetooth;
#endif

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class ParametrizacionVM : ObservableObject
    {
        [ObservableProperty]
        string nombrePDA;

        public ParametrizacionVM()
        {
            NombrePDA = ObtenerNombreBluetooth();
        }

        private string ObtenerNombreBluetooth()
        {
#if ANDROID
            try
            {
                var adapter = BluetoothAdapter.DefaultAdapter;

                if (adapter != null)
                {
                    return adapter.Name;
                }
                else
                {
                    return "Bluetooth no disponible";
                }
            }
            catch
            {
                return "No se pudo obtener el nombre";
            }
#else
            return "No disponible";
#endif
        }

        [RelayCommand]
        async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
