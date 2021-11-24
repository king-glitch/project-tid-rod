using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TidRod.ViewModels.Host
{
    [QueryProperty(nameof(PinLocation), nameof(PinLocation))]
    [QueryProperty(nameof(CarId), nameof(CarId))]
    public class HostPinLocationViewModel : BaseViewModel
    {
        public Command SubmitPinLocationCommand { get; }

        private string _pinLocation;
        private string _id;

        public string PinLocation
        {
            get => _pinLocation;
            set => SetProperty(ref _pinLocation, value);

        }

        public string CarId
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public HostPinLocationViewModel()
        {
            SubmitPinLocationCommand = new Command(OnSubmitPinLocation);
        }

        private async void OnSubmitPinLocation()
        {
            var id = string.IsNullOrEmpty(CarId) ? "" : $"&{nameof(CarId)}={CarId}&ReInitialize=1";
            var pinLocation = string.IsNullOrEmpty(PinLocation) ? "" : $"?{nameof(PinLocation)}={PinLocation}";
            await Shell.Current.GoToAsync($"..{pinLocation}{id}");
        }
    }
}
