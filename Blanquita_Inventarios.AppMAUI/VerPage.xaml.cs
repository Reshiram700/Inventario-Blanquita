using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class VerPage : ContentPage
{
	public VerPage(VerVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}