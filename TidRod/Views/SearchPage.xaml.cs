using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.Views
{
    public partial class SearchPage : ContentPage
    {
        private CancellationTokenSource cts;

        readonly SearchViewModel _viewModel;

        public SearchPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SearchViewModel();
            InitializeMap();
        }

        private async void InitializeMap()
        {
            List<PinInfo> pins = new List<PinInfo>();
            await this._viewModel.GetCarsAroundUserCommand();
            var cars = _viewModel?.Cars;

            foreach (var car in cars)
            {
                Geocoder geoCoder = new Geocoder();
                var pos = car.PinLocation.Split(',');
                Position position =
                    new Position(float.Parse(pos[0]), float.Parse(pos[1]));
                IEnumerable<string> possibleAddresses =
                    await geoCoder.GetAddressesForPositionAsync(position);
                string address = possibleAddresses.FirstOrDefault();
                pins
                    .Add(new PinInfo {
                        Address = address,
                        Position = position,
                        Name = car.Name,
                        Icon = ""
                    });
            }

            // get the location
            var location = await GetCurrentLocation();

            // go to current location
            MainMap
                .MoveToRegion(MapSpan
                    .FromCenterAndRadius(new Position(location.Latitude,
                        location.Longitude),
                    Distance.FromKilometers(5)));

            // Update Map pins
            MainMap.ItemsSource = pins;
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
                await DisplayAlert("Faild",
                "Please give location permission",
                "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
            return null;
        }
    }
}
