using System;
using System.Linq;
using TidRod.Models;
using TidRod.Utils;
using TidRod.Views.Auth;
using TidRod.Views.General;
using Xamarin.Forms;

namespace TidRod.ViewModels.Auth
{
    public class RegisterationViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterPage { get; }

        private string _email;
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _password;
        private string _confirmPassword;

        private string _emailError;
        private string _firstNameError;
        private string _lastNameError;
        private string _phoneError;
        private string _passwordError;
        private string _confirmPasswordError;

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }
        public string FirstNameError
        {
            get => _firstNameError;
            set => SetProperty(ref _firstNameError, value);
        }
        public string LastNameError
        {
            get => _lastNameError;
            set => SetProperty(ref _lastNameError, value);
        }
        public string PhoneError
        {
            get => _phoneError;
            set => SetProperty(ref _phoneError, value);
        }
        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }
        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set => SetProperty(ref _confirmPasswordError, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public RegisterationViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterPage = new Command(OnRegisterClicked);
        }
        private void ResetForms()
        {
            EmailError = "";
            FirstNameError = "";
            LastNameError = "";
            PhoneError = "";
            PasswordError = "";
            ConfirmPasswordError = "";
        }

        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/{nameof(LoginPage)}");
        }

        private async void OnRegisterClicked(object obj)
        {
            // reset previous forms;
            this.ResetForms();

            bool IsError = false;

            // check if email is empty;

            if (string.IsNullOrEmpty(Email))
            {
                EmailError = MainLanguage.AUTHENTICATION_EMAIL_EMPTY;
                IsError = true;
            }
            // check if email is valid;
            else if (!TidRodUtilitiles.IsValidEmail(Email))
            {
                // show invalid error;
                EmailError = MainLanguage.PROFILE_EMAIL_INVALID;
                IsError = true;
            }
            else
            {

                // check if email is unique;
                var users = await this.UserDataStore.GetUsersAsync();

                if (users.ToList().Where(u => u.Email.ToLower() == Email.ToLower()).Count() > 0)
                {
                    // show already exist email;
                    EmailError = MainLanguage.PROFILE_EMAIL_EXISTS;
                    IsError = true;
                }
            }

            if (string.IsNullOrEmpty(Password))
            {
                PasswordError = MainLanguage.AUTHENTICATION_PASSWORD_EMPTY;
                IsError = true;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ConfirmPasswordError = MainLanguage.AUTHENTICATION_CONFIRM_PASSWORD_EMPTY;
                IsError = true;
            }

            if (string.IsNullOrEmpty(Phone))
            {
                PhoneError = MainLanguage.AUTHENTICATION_PHONE_EMPTY;
                IsError = true;
            }
            else if (!TidRodUtilitiles.IsValidPhone(Phone))
            {
                // show invalid error;
                PhoneError = MainLanguage.AUTHENTICATION_PHONE_INCORRECTED;
                IsError = true;
            }
            else
            {
                // check if the phone number is already registered;
                var users = await this.UserDataStore.GetUsersAsync();
                if (users.ToList().Where(u => u.Phone.ToLower() == Phone.ToLower()).Count() > 0)
                {
                    // show already exist phone;
                    PhoneError = MainLanguage.AUTHENTICATION_PHONE_EXISTS;
                    IsError = true;
                }
            }

            // check if first name is empty;
            if (string.IsNullOrEmpty(FirstName))
            {
                FirstNameError = MainLanguage.AUTHENTICATION_FIRST_NAME_EMPTY;
                IsError = true;
            }

            // check if last name is empty;

            if (string.IsNullOrEmpty(LastName))
            {
                LastNameError = MainLanguage.AUTHENTICATION_LAST_NAME_EMPTY;
                IsError = true;
            }

            // if the errors presist return;

            if (IsError)
            {
                return;
            }

            // check if password are the same

            if (ConfirmPassword != Password)
            {
                ConfirmPasswordError = MainLanguage.AUTHENTICATION_PASSWORD_NOT_MATCH;
                PasswordError = MainLanguage.AUTHENTICATION_PASSWORD_NOT_MATCH;
                IsError = true;
            }

            // if the errors presist return;

            if (IsError)
            {
                return;
            }

            // add user info to the database.

            await this.UserDataStore.AddUserAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = FirstName.ToLower(),
                LastName = LastName.ToLower(),
                Email = Email.ToLower(),
                Password = Password,
                Phone = Phone
            });

            // go to the main page;

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            await Application.Current.MainPage.DisplayAlert(
                    MainLanguage.GENERAL_SUCCESSFULLY_TITLE,
                    MainLanguage.AUTHENTICATION_REGISTER_SUCCESSFULLY, "yeah");
        }


    }
}