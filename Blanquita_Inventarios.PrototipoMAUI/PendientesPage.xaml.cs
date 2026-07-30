using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class PendientesPage : ContentPage
{
	public PendientesPage(PendientesVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}