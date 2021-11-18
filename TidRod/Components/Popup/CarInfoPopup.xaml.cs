using Rg.Plugins.Popup.Extensions;
using System;
using TidRod.Models;
using Xamarin.Forms.Xaml;

namespace TidRod.Components.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarInfoPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public CarInfoPopup()
        {
            InitializeComponent();
            //InitializeImages();

            BindingContext = this;


        }

        private void InitializeImages()
        {
            var cars = (Car)BindingContext;
            Console.WriteLine(cars.ToString());

        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}