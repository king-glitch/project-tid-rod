﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TidRod.Views;
using TidRod.Views.Search;
using Xamarin.Forms;

namespace TidRod.ViewModels.General
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
