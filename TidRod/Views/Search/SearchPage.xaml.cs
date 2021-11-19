using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            InitializeMap();
        }

        private async void InitializeMap()
        {
            // initialize

            List<CustomPin> pins = new List<CustomPin>();
            await this._viewModel.GetCarsAroundUserCommand();
            var cars = _viewModel?.Cars;


            // check if null
            if (cars == null)
            {
                cars = new ObservableCollection<Car>();
            }

            // pin all locations
            foreach (var car in cars)
            {

                // call geo code
                Geocoder geoCoder = new Geocoder();

                //split lan, long
                var pos = car.PinLocation.Split(',');

                // get position
                Position position =
                    new Position(float.Parse(pos[0]), float.Parse(pos[1]));

                // get possible addresses
                IEnumerable<string> possibleAddresses =
                    await geoCoder.GetAddressesForPositionAsync(position);

                // get first address
                string address = possibleAddresses.FirstOrDefault();


                // add to the pin
                pins
                    .Add(new CustomPin
                    {
                        ClassId = car.Id,
                        Address = address,
                        Position = position,
                        Name = car.Name,
                        Type = PinType.Place
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

        private async void MapPinClicked(object sender, EventArgs e)
        {
            var pin = (CustomPin)sender;

            var id = pin.ClassId.ToString();
            var car = await _viewModel.CarDataStore.GetCarAsync(id);
            await Navigation.PushPopupAsync(new CarInfoPopup
            {
                BindingContext = car
            });
        }

        private async void ToolbarFilterCarsClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new CarsFilterPopup());
        }
    }
}
