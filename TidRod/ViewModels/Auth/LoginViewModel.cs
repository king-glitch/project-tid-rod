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

        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterPage = new Command(OnRegisterClicked);
        }

        private async Task<bool> Login(string email, string password)
        {

            List<User> users = (await this.UserDataStore.GetUsersAsync(true)).ToList();

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("no email");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("no password");
                return false;
            }

            try
            {
                var user = users.Find(u => u?.Email?.ToLower() == email?.ToLower());
                
                if (user == null)
                {
                    return false;
                }


                if (user.Password == password)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return false;
        }

        private async void OnLoginClicked(object obj)
        {
            bool logged = await this.Login(Email, Password);

            if (logged)
            {
                await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
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