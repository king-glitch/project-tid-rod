using Rg.Plugins.Popup.Extensions;
using System;
using TidRod.Models;
using TidRod.ViewModels.Profile;
using TidRod.Views.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Components.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarInfoPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public CarInfoPopup()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            await Shell.Current.GoToAsync($"{nameof(HostProfilePage)}?{nameof(HostProfileViewModel.CarId)}={((Car)BindingContext).Id}");
        }
    }
}