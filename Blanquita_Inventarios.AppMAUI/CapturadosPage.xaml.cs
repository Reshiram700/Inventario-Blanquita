using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class CapturadosPage : ContentPage
{
	public CapturadosPage(CapturadosVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}