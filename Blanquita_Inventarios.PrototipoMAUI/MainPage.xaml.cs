using Blanquita_Inventarios.PrototipoMAUI.ViewModel;

namespace Blanquita_Inventarios.PrototipoMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainVM vm)
        {
            InitializeComponent();
            BindingContext = vm;

            NavigationPage.SetHasNavigationBar(this, false);
        }
    }

}
