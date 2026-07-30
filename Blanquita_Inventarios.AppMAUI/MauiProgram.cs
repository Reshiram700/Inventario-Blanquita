using Blanquita_Inventarios.AppMAUI.ViewModels;
using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;


namespace Blanquita_Inventarios.AppMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
             .UseMauiApp<App>()
             .UseUserDialogs(() =>
             {
                 //setup your default styles for dialogs
                 AlertConfig.DefaultBackgroundColor = Colors.Purple;
#if ANDROID
        AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular.ttf";
#else
                 AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular";
#endif

                 ToastConfig.DefaultCornerRadius = 15;
             })
             .UseBarcodeReader()
             .UseMauiCommunityToolkit();

            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
        #if ANDROID
                handler.PlatformView.Background = null;
        #endif
            });


            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginVM>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainVM>();

            builder.Services.AddTransient<ParametrizacionPage>();
            builder.Services.AddTransient<ParametrizacionVM>();

            builder.Services.AddTransient<ConfiguracionPage>();
            builder.Services.AddTransient<ConfiguracionVM>();

            builder.Services.AddTransient<CapturarPage>();
            builder.Services.AddTransient<CapturarVM>();

            builder.Services.AddTransient<BarcodePage>();
            builder.Services.AddTransient<BarcodeVM>();

            builder.Services.AddTransient<CapturadosPage>();
            builder.Services.AddTransient<CapturadosVM>();

            builder.Services.AddTransient<PendientesPage>();
            builder.Services.AddTransient<PendientesVM>();

            builder.Services.AddTransient<VerPage>();
            builder.Services.AddTransient<VerVM>();

#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
