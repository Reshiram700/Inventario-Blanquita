using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class ConfiguracionPage : ContentPage
{
	public ConfiguracionPage(ConfiguracionVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}