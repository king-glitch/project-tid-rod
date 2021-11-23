using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TidRod.ViewModels.Host
{
    [QueryProperty(nameof(PinLocation), nameof(PinLocation))]
    public class HostPinLocationViewModel : BaseViewModel
    {
        public Command SubmitPinLocationCommand { get; }

        private string _pinLocation;

        public string PinLocation
        {
            get => _pinLocation;
            set
            {
                SetProperty(ref _pinLocation, value);
            }
        }

        public HostPinLocationViewModel()
        {
            SubmitPinLocationCommand = new Command(OnSubmitPinLocation);
        }

        private async void OnSubmitPinLocation()
        {
            var pinLocation = string.IsNullOrEmpty(PinLocation) ? "" : $"?{nameof(HostPinLocationViewModel.PinLocation)}={PinLocation}";
            await Shell.Current.GoToAsync($"..{pinLocation}");
        }
    }
}
