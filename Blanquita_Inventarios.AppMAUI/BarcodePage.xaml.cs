using Blanquita_Inventarios.AppMAUI.ViewModels;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Blanquita_Inventarios.AppMAUI;

public partial class BarcodePage : ContentPage
{
	public BarcodePage(BarcodeVM vm)
	{
		InitializeComponent();
        BindingContext = vm;

        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
    }

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            string valor = e.Results[0].Value;
            lblResultado.Text = valor;

            ((BarcodeVM)BindingContext).DatoScan = valor;
        });
    }
}