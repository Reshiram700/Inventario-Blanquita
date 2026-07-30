using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class CatalogosPage : ContentPage
{
	public CatalogosPage(CatalogosVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}