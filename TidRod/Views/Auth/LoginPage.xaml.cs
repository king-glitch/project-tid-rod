using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TidRod.Models;
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

        private void EmailEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            _baseView.Email = EmailEntry.Text;
        }

        private void PasswordEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            _baseView.Password = PasswordEntry.Text;
        }
    }
}