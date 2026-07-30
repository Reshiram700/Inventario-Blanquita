using Blanquita_Inventarios.PrototipoMAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.PrototipoMAUI.ViewModel
{
    public partial class CapturadosVM : ObservableObject
    {
        [ObservableProperty]
        List<Generico> listado;

        [ObservableProperty]
        ObservableCollection<ItemMarbeteVM> getMarbetes;

        public CapturadosVM()
        {
            Listado = new List<Generico> { 
                new Generico { Marbete = "1", Estatus = "Cerrado", Visible = true},
                new Generico { Marbete = "2", Estatus = "Cerrado", Visible = true},
                new Generico { Marbete = "3", Estatus = "Descargado", Visible = false},
                new Generico { Marbete = "4", Estatus = "Cerrado", Visible = true},
                new Generico { Marbete = "5", Estatus = "Descargado", Visible = false}
            };

            var list = Listado.Select(a => new ItemMarbeteVM { 
                Marbete = a.Marbete,
                Estatus = a.Estatus,
                Visible = a.Visible
            }).ToList();

            GetMarbetes = new ObservableCollection<ItemMarbeteVM>(list);
        }
    }
}
