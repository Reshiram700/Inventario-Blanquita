using Blanquita_Inventarios.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class VerVM : ObservableObject
    {
        private readonly DatabaseService _database;

        [ObservableProperty]
        List<DatosBarcode> listado;

        [ObservableProperty]
        ObservableCollection<ItemDatosCatalogoVM> getProductos;

        public VerVM()
        {
            _database = new DatabaseService(FileSystem.AppDataDirectory);            
        }

        private async void Carga_Productos()
        {
            var listadoArticulos = await _database.Obtener_SAP_Articulos_Async();
            if (listadoArticulos != null && listadoArticulos.Count > 0)
            {
                var list = listadoArticulos.Select(a => new ItemDatosCatalogoVM
                {
                    Codigo = "",
                    ItemCode = a.ItemCode,
                    Descripcion = a.ItemName,
                    Uom = "",
                    Precio = a.Price
                }).Take(50).ToList();

                GetProductos = new ObservableCollection<ItemDatosCatalogoVM>(list);
            }
            else
            {
                await Shell.Current.DisplayAlert("", "No hay información", "OK");
            }            
        }

        private async void Carga_Codigos()
        {
            var listadoArticulos = await _database.Obtener_SAP_Barcodes_Async();
            if (listadoArticulos != null && listadoArticulos.Count > 0)
            {
                var list = listadoArticulos.Select(a => new ItemDatosCatalogoVM
                {
                    Codigo = a.BcdCode,
                    ItemCode = a.ItemCode,
                    Descripcion = "",
                    Uom = a.UomCode,
                    Precio = 0
                }).Take(50).ToList();

                GetProductos = new ObservableCollection<ItemDatosCatalogoVM>(list);
            }
            else
            {
                await Shell.Current.DisplayAlert("", "No hay información", "OK");
            }
        }

        [RelayCommand]
        async void Productos()
        {
            Carga_Productos();
        }

        [RelayCommand]
        async void Codigos()
        {
            Carga_Codigos();
        }

        [RelayCommand]
        async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
