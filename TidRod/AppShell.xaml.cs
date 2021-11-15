using System;
using System.Collections.Generic;
using TidRod.ViewModels;
using TidRod.Views;
using TidRod.Views.Auth;
using Xamarin.Forms;

namespace TidRod
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterationPage), typeof(RegisterationPage));
        }
    }
}