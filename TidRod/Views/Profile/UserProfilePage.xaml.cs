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
    public partial class UserProfilePage : ContentPage
    {
        private UserProfileViewModel _viewModel;

        public UserProfilePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new UserProfileViewModel();
            BackgroundColor = Color.White;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void ToolbarSettingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(UserProfileSettingsPage)}");
        }
    }
}
