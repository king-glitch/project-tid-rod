using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TidRod.Components.Map;
using TidRod.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.ViewModels
{
    [QueryProperty(nameof(Price), nameof(Price))]
    [QueryProperty(nameof(Obometer), nameof(Obometer))]
    [QueryProperty(nameof(Gear), nameof(Gear))]
    public class SearchViewModel : BaseViewModel
    {

        private int _price;
        private int _obometer;
        private string _gear;
        private List<CustomPin> _pins;
        public ICommand OpenWebCommand { get; }

        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public List<CustomPin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        public int Obometer
        {
            get => _obometer;
            set => SetProperty(ref _obometer, value);
        }

        public string Gear
        {
            get => _gear;
            set
            {
                InitializeMap();
                SetProperty(ref _gear, value);
            }
        }

        public List<Car> Cars { get; }

        public Command LoadCarsCommand { get; }

        public SearchViewModel()
        {
            Title = "Cars Around You";

            Cars = new List<Car>();

            LoadCarsCommand = new Command(async () => await GetCarsAroundUserCommand());
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            InitializeMap();
            Gear = "Both";
            Obometer = -1;
            Price = -1;
            await GetCarsAroundUserCommand();
           
        }

        private async void InitializeMap()
        {
            await Task.Delay(500);
            // initialize
            List<CustomPin> pins = new List<CustomPin>();
            await this.GetCarsAroundUserCommand();
            var cars = this?.Cars;

            // check if null
            if (cars == null)
            {
                cars = new List<Car>();
            }


            bool check = true;

            try
            {
                cars =
                cars
                    .FindAll(car =>
                    {

                        if (Price > 0)
                        {
                            check = check && car.Price <= Price;
                        }

                        if (Obometer > 0)
                        {
                            check = check && car.Obometer <= Obometer;
                        }

                        if (Gear == "Both")
                        {
                            return check;
                        }

                        if (!string.IsNullOrEmpty(Gear))
                        {
                            return check && car.Gear == (Gear == "Automatic"
                                ? CarTransmission.Automatic
                                : CarTransmission.Manual);
                        }

                        return check;
                    });

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

                // Update Map pins
                Pins = pins;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }



        public async Task GetCarsAroundUserCommand()
        {
            IsBusy = true;

            try
            {
                Cars.Clear();
                var cars = await CarDataStore.GetCarsAsync(true);
                foreach (var car in cars)
                {
                    Cars.Add(car);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
