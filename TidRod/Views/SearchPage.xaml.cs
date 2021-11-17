using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TidRod.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.Views
{
    public partial class SearchPage : ContentPage
    {
        private CancellationTokenSource cts;

        public SearchPage()
        {
            InitializeComponent();
            InitializeMap();
        }

        private async void InitializeMap()
        {
            List<PinInfo> pins = new List<PinInfo>();

            pins
                .Add(new PinInfo {
                    Address = "Bangkok University",
                    Position =
                        new Position(14.065700580211209, 100.6099274604641),
                    Name = "Yang Noey"
                });

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

            //MainMap.ItemsSource = new List<>
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
