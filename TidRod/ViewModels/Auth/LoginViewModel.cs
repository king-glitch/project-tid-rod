using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Views;
using TidRod.Views.Auth;
using TidRod.Views.General;
using TidRod.Views.Search;
using Xamarin.Forms;

namespace TidRod.ViewModels.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterPage { get; }

        private string _email;
        private string _password;

        private string _emailError;
        private string _passwordError;
        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }
        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterPage = new Command(OnRegisterClicked);

            this.PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private void ResetForms()
        {
            PasswordError = "";
            EmailError = "";
        }

        private async Task<bool> LoginValidations(string email, string password)
        {
            this.ResetForms();
            List<User> users = (await this.UserDataStore.GetUsersAsync(true)).ToList();

            if (string.IsNullOrEmpty(email))
            {
                EmailError = MainLanguage.AUTHENTICATION_EMAIL_EMPTY;
            }

            if (string.IsNullOrEmpty(password))
            {
                PasswordError = MainLanguage.AUTHENTICATION_PASSWORD_EMPTY;
            }

            if (!string.IsNullOrWhiteSpace(EmailError) || !string.IsNullOrEmpty(PasswordError))
            {
                return false;
            }

            try
            {

                var user = users.Find(u => u?.Email?.ToLower() == email?.ToLower());

                if (user == null)
                {
                    EmailError = MainLanguage.AUTHENTICATION_EMAIL_INCORRECTED;
                    return false;
                }


                if (user.Password == password)
                {
                    App.CurrentSession = user.Id;

                    return true;
                }

                PasswordError = MainLanguage.AUTHENTICATION_PASSWORD_INCORRECTED;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            await Application.Current.MainPage.DisplayAlert(MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE, MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC, "Fuck");
            return false;
        }

        private async void OnLoginClicked()
        {
            IsBusy = true;
            bool logged = await this.LoginValidations(Email, Password);
            IsBusy = false;

            if (logged)
            {
                await Shell.Current.GoToAsync($"..");

                await Shell.Current.GoToAsync($"///{nameof(SearchPage)}");
            }

            return;

        }

        private async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/{nameof(RegisterationPage)}");
        }


    }
}