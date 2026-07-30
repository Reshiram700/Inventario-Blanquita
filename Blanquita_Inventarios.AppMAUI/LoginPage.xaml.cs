using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}