
using Blanquita_Inventarios.PrototipoMAUI.ViewModel;
using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;

namespace Blanquita_Inventarios.PrototipoMAUI
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();


            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<LoginVM>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainVM>();

            builder.Services.AddTransient<ParametrizacionPage>();
            builder.Services.AddTransient<ParametrizacionVM>();

            builder.Services.AddTransient<ConfiguracionPage>();
            builder.Services.AddTransient<ConfiguracionVM>();

            builder.Services.AddTransient<CatalogosPage>();
            builder.Services.AddTransient<CatalogosVM>();

            builder.Services.AddTransient<CapturarPage>();
            builder.Services.AddTransient<CapturarVM>();

            builder.Services.AddTransient<CapturadosPage>();
            builder.Services.AddTransient<CapturadosVM>();

            builder.Services.AddTransient<PendientesPage>();
            builder.Services.AddTransient<PendientesVM>();

            builder.Services.AddTransient<BorrarPage>();
            builder.Services.AddTransient<BorrarVM>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
