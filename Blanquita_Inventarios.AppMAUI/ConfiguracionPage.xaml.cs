using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class ConfiguracionPage : ContentPage
{
	public ConfiguracionPage(ConfiguracionVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}