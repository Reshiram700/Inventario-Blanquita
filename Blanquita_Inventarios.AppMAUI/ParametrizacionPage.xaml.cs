using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class ParametrizacionPage : ContentPage
{
	public ParametrizacionPage(ParametrizacionVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}