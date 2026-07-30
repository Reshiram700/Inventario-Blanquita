using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class BorrarPage : ContentPage
{
	public BorrarPage(BorrarVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}