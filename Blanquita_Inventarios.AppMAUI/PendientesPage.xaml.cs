using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class PendientesPage : ContentPage
{
	public PendientesPage(PendientesVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}