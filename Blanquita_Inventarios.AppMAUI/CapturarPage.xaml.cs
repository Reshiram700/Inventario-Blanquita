using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI;

public partial class CapturarPage : ContentPage
{
	public CapturarPage(CapturarVM vm)
	{
		InitializeComponent();
        BindingContext = vm;

        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        entMarbete.Focus();

        ((CapturarVM)BindingContext).ControlMarbete = this.entMarbete;
        ((CapturarVM)BindingContext).ControlBarcode = this.entCodigo;
    }

    protected override bool OnBackButtonPressed()
    {
        // Return true to prevent the back button from navigating back.
        // Return false to allow the default back navigation behavior.
        return true;
    }
}