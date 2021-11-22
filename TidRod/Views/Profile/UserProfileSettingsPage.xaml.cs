using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TidRod.ViewModels.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfileSettingsPage : ContentPage
    {
        private UserProfileSettingsViewModel _viewModel;

        public UserProfileSettingsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new UserProfileSettingsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
