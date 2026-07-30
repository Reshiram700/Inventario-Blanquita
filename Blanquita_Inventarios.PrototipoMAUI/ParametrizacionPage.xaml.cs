using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class ParametrizacionPage : ContentPage
{
	public ParametrizacionPage(ParametrizacionVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}