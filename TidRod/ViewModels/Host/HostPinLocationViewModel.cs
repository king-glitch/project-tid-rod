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
            // organize car image;
            var id = string.IsNullOrEmpty(CarId) ? "" : $"&{nameof(CarId)}={CarId}&ReInitialize=1";
            // pin location;
            var pinLocation = string.IsNullOrEmpty(PinLocation) ? "" : $"?{nameof(PinLocation)}={PinLocation}";

            // send back to the previous page;
            await Shell.Current.GoToAsync($"..{pinLocation}{id}");
        }
    }
}
