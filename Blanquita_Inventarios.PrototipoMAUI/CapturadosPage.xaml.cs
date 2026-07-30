using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class CapturadosPage : ContentPage
{
	public CapturadosPage(CapturadosVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}