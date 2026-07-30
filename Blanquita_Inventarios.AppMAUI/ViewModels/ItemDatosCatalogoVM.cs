using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.AppMAUI.ViewModels
{
    public partial class ItemDatosCatalogoVM : ObservableObject
    {
        [ObservableProperty]
        string codigo;

        [ObservableProperty]
        string itemCode;

        [ObservableProperty]
        string descripcion;

        [ObservableProperty]
        string uom;

        [ObservableProperty]
        decimal precio;
    }
}
