using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TidRod.ViewModels.Host;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Host
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostPinLocationPage : ContentPage
    {
        private CancellationTokenSource cts;
        private readonly HostPinLocationViewModel _viewModel;
        public HostPinLocationPage()
        {
            InitializeComponent();
            InitializeMap();
            BindingContext = _viewModel = new HostPinLocationViewModel();

        }

        private async void InitializeMap()
        {
            double zoom = 17;
            double degree = 360 / (Math.Pow(2, zoom));
            Location location = await this.GetCurrentLocation();
            Position position = new Position(location.Latitude, location.Longitude);
            PositionMap.MoveToRegion(new MapSpan(position, degree, degree));
        }

        private void MapPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = (Xamarin.Forms.Maps.Map)sender;
            if (m.VisibleRegion != null)
            {
                _viewModel.PinLocation = $"{m.VisibleRegion.Center.Latitude},{m.VisibleRegion.Center.Longitude}";
            }
        }

        private async Task<Location> GetCurrentLocation()
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewModel?.PinLocation))
                {
                    var pos = _viewModel.PinLocation.Split(',');
                    var placemarks = await Geocoding.GetPlacemarksAsync(double.Parse(pos[0]), double.Parse(pos[1]));
                    var placemark = placemarks?.FirstOrDefault();
                    return placemark.Location;
                }
                else
                {
                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    cts = new CancellationTokenSource();
                    return await Geolocation.GetLocationAsync(request, cts.Token);
                }

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
                await DisplayAlert("Faild", ex.StackTrace, "OK");
            }
            return null;
        }
    }
}