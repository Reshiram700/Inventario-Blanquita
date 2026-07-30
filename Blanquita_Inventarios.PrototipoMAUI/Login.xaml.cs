using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI;

public partial class Login : ContentPage
{
	public Login(LoginVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}