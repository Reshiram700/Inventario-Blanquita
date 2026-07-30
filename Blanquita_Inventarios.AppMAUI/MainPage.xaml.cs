using Blanquita_Inventarios.AppMAUI.ViewModels;

namespace Blanquita_Inventarios.AppMAUI
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
