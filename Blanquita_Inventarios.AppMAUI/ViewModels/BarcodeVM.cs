using Blanquita_Inventarios.AppMAUI.Generics;
using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class BarcodeVM : ObservableObject, IQueryAttributable
    {
        private readonly DatabaseService _database;

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Datos = new DatosBarcode();

            if (query.ContainsKey("Barcode_Tipo"))
            {
                Datos.Tipo = (int)query["Barcode_Tipo"];
            }
        }

        [ObservableProperty]
        string datoScan;

        [ObservableProperty]
        string marbete;

        [ObservableProperty]
        DatosBarcode datos;

        public BarcodeVM()
        {
            _database = new DatabaseService(FileSystem.AppDataDirectory);

            DatoScan = string.Empty;
        }

        [RelayCommand]
        async void Tomar()
        {

            if (!String.IsNullOrEmpty(DatoScan))
            {
                if (Datos.Tipo == 1)
                    Datos.Marbete = int.Parse(DatoScan);
                else
                {
                    Datos = await _database.Buscar_DatosScanBarcodeProducto_Async(DatoScan);
                    Datos.Tipo = 2;
                }

                var navigationParameter = new Dictionary<string, object>
                {
                    { "Barcode_Datos", Datos }
                };

                await Shell.Current.GoToAsync("..", navigationParameter);
            }
            else
            {
                await Shell.Current.DisplayAlert("", "No se identifico el Código de Barras", "OK");
            }
        }

        [RelayCommand]
        async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
