namespace Blanquita_Inventarios.AppMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ParametrizacionPage), typeof(ParametrizacionPage));
            Routing.RegisterRoute(nameof(ConfiguracionPage), typeof(ConfiguracionPage));
            Routing.RegisterRoute(nameof(CapturarPage), typeof(CapturarPage));
            Routing.RegisterRoute(nameof(BarcodePage), typeof(BarcodePage));
            Routing.RegisterRoute(nameof(CapturadosPage), typeof(CapturadosPage));
            Routing.RegisterRoute(nameof(PendientesPage), typeof(PendientesPage));
            Routing.RegisterRoute(nameof(VerPage), typeof(VerPage));
        }
    }
}
