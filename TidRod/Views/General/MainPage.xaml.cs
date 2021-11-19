using System;
using TidRod.ViewModels;
using TidRod.ViewModels.General;
using TidRod.Views.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.General
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MainPageViewModel();

            // hide title bar
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void GotoLoginPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        private async void GotoRegisterationPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterationPage)}");
        }
    }
}