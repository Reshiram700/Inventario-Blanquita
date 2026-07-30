using Blanquita_Inventarios.AppMAUI.Generics;
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
    public partial class CapturadosVM : ObservableObject
    {
        private readonly DatabaseService _database;

        [ObservableProperty]
        ObservableCollection<ItemMarbeteVM> getMarbetes;

        [ObservableProperty]
        int capturados;

        [ObservableProperty]
        int descargados;

        [ObservableProperty]
        int articulos;

        public CapturadosVM()
        {
            _database = new DatabaseService(FileSystem.AppDataDirectory);
            Articulos = 0;
            Carga_Marbetes();
        }

        private async void Carga_Marbetes()
        {
            var response = await _database.Get_MarbetesCapturadosProductos();
            if (response.ExecutionOK)
            {

                var list = response.Data.Select(a => new ItemMarbeteVM
                {
                    IdZona = a.IdZona,
                    Marbete = a.Marbete,
                    Estatus = a.Estatus == 1 ? "En Captura" : ( a.Estatus == 2 ? "Cerrado" : "Enviado"),
                    CantidadProductos = a.CantidadProductos,
                    Visible = a.Estatus == 2
                }).ToList();

                Articulos = list.Sum(a => a.CantidadProductos);

                GetMarbetes = new ObservableCollection<ItemMarbeteVM>(list);
            }

            var parametrizacion = await _database.Obtener_SI_Parametrizacion_Async();
            if (parametrizacion.Count > 0)
            {
                var parametros = parametrizacion.Where(s => s.IdParametrizacion == 1).FirstOrDefault();
                if (parametros != null)
                {
                    Capturados = parametros.Capturados;
                    Descargados = parametros.Descargados;
                }
            }
        }

        [RelayCommand]
        async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
