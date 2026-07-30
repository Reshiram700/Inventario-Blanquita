using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class CapturarPage : ContentPage
{
	public CapturarPage(CapturarVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}