using TidRod.ViewModels.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostProfilePage : ContentPage
    {
        private HostProfileViewModel _viewModel;

        public HostProfilePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new HostProfileViewModel();
            Title = "Profile";
            BackgroundColor = Color.White;
        }
    }
}
