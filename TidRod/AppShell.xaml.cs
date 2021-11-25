using System;
using System.Collections.Generic;
using TidRod.ViewModels;
using TidRod.Views;
using TidRod.Views.Auth;
using TidRod.Views.General;
using TidRod.Views.Host;
using TidRod.Views.Profile;
using Xamarin.Forms;

namespace TidRod
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // general
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            // authentication
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterationPage), typeof(RegisterationPage));

            // profile
            Routing.RegisterRoute(nameof(HostProfilePage), typeof(HostProfilePage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(UserProfileSettingsPage), typeof(UserProfileSettingsPage));

            // host
            Routing.RegisterRoute(nameof(HostCarsPage), typeof(HostCarsPage));
            Routing.RegisterRoute(nameof(HostCarAddPage), typeof(HostCarAddPage));
            Routing.RegisterRoute(nameof(HostCarUpdatePage), typeof(HostCarUpdatePage));
            Routing.RegisterRoute(nameof(HostPinLocationPage), typeof(HostPinLocationPage));
        }
    }
}
