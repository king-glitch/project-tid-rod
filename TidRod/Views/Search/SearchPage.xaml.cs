using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using TidRod.Components.Map;
using TidRod.Components.Popup;
using TidRod.Models;
using TidRod.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.Views.Search
{
    public partial class SearchPage : ContentPage
    {

        private CancellationTokenSource cts;
        readonly SearchViewModel _viewModel;

        public SearchPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SearchViewModel();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

            // get the location
            var location = await GetCurrentLocation();

            // go to current location
            MainMap
                .MoveToRegion(MapSpan
                    .FromCenterAndRadius(new Position(location.Latitude,
                        location.Longitude),
                    Distance.FromKilometers(5)));
        }

        private async Task<Location> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request =
                    new GeolocationRequest(GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                return await Geolocation.GetLocationAsync(request, cts.Token);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Faild", "Please give location permission", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
            return null;
        }

        private async void MapPinClicked(object sender, EventArgs e)
        {
            var pin = (CustomPin)sender;

            var id = pin.ClassId.ToString();
            var car = await _viewModel.CarDataStore.GetCarAsync(id);
            await Navigation.PushPopupAsync(new CarInfoPopup { BindingContext = car });
        }

        private async void ToolbarFilterCarsClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new CarsFilterPopup());
        }
    }
}
