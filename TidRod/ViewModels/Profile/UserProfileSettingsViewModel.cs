using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TidRod.Models;
using Xamarin.Forms;

namespace TidRod.ViewModels.Profile
{
    public class UserProfileSettingsViewModel : BaseViewModel
    {
        public Command UpdateUserProfile { get; }
        private string _firstName;
        private string _lastName;
        private string _email;

        public User CurrentUser { get; set; }
        public string EmailError { get; set; }
        public string FirstNameError { get; set; }
        public string LastNameError { get; set; }

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

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private void ResetForms()
        {
            EmailError = "";
            FirstNameError = "";
            LastNameError = "";
        }

        public UserProfileSettingsViewModel()
        {
            UpdateUserProfile = new Command(OnUpdate);
        }

        public async void OnAppearing()
        {
            CurrentUser = await this.UserDataStore.GetUserAsync(App.CurrentSesstion);

            Email = CurrentUser.Email;
            FirstName = CurrentUser.FirstName;
            LastName = CurrentUser.LastName;

        }

        public async void OnUpdate()
        {

            // reset all errors;
            this.ResetForms();


            // check if email null;
            if (string.IsNullOrEmpty(Email))
            {
                EmailError = MainLanguage.PROFILE_EMAIL_EMPTY;
            }

            // check if first name null;
            if (string.IsNullOrEmpty(FirstName))
            {
                FirstNameError = MainLanguage.PROFILE_FIRST_NAME_EMPTY;
            }


            // check if last name null;
            if (string.IsNullOrEmpty(LastName))
            {
                LastNameError = MainLanguage.PROFILE_LAST_NAME_EMPTY;
            }


            // check all errors if presist;
            bool IsError =
                !string.IsNullOrWhiteSpace(FirstNameError) ||
                !string.IsNullOrWhiteSpace(LastNameError) ||
                !string.IsNullOrWhiteSpace(EmailError);

            if (IsError)
            {
                return;
            }


            // try to update;
            try
            {
                // get user current info;
                var user = CurrentUser;

                var users = await this.UserDataStore.GetUsersAsync();

                // check if email already existed and not same as the current one;

                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE,
                        MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC, "Fuck");
                    return;
                }

                if (
                    Email != user.Email && !this.IsValidEmail(Email))
                {
                    // show invalid error;
                    EmailError = MainLanguage.PROFILE_EMAIL_INVALID;
                    return;
                }

                if (Email != user.Email && users.ToList().Where(u => u.Email.ToLower() == Email.ToLower()).Count() > 0)
                {
                    // show already exist email;
                    EmailError = MainLanguage.PROFILE_EMAIL_EXISTS;
                    return;
                }

                // update user data;

                user.FirstName = FirstName;
                user.LastName = LastName;
                user.Email = Email;

                await this.UserDataStore.UpdateUserAsync(user);
                await Application.Current.MainPage.DisplayAlert(
                        MainLanguage.GENERAL_SUCCESSFULLY_TITLE,
                        MainLanguage.PROFILE_UPDATE_SUCCESSFULLY, "yeah");
                await Shell.Current.GoToAsync("..");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            // something went wrong;
            await Application.Current.MainPage.DisplayAlert(MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE, MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC, "Fuck");
            return;
        }

        private bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
