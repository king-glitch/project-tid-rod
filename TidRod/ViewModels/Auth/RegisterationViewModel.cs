using System;
using System.Collections.Generic;
using System.Text;
using TidRod.Views;
using TidRod.Views.Auth;
using Xamarin.Forms;

namespace TidRod.ViewModels.Auth
{
    public class RegisterationViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterPage { get; }

        public RegisterationViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterPage = new Command(OnRegisterClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/{nameof(LoginPage)}");
        }

        private async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/{nameof(RegisterationPage)}");
        }
    }
}