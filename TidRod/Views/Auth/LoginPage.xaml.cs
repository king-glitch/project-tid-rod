using TidRod.ViewModels.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _baseView;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _baseView = new LoginViewModel();
        }
    }
}
