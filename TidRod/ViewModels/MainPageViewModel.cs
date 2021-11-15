using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TidRod.Views;
using Xamarin.Forms;

namespace TidRod.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel()
        {
            Title = "Main Page";

        }
       
        public async void OnAppearing()
        {
            bool LoggedIn = false;
            if (LoggedIn)
            {
                await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
            }
        }
    }
}