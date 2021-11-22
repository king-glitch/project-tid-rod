using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TidRod.Models;
using Xamarin.Forms;

namespace TidRod.ViewModels.Host
{
    [QueryProperty(nameof(CarId), nameof(CarId))]
    public class HostCarUpdateViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string _name;
        public List<string> _images;
        public int _price;
        public int _obometer;
        public CarTransmission _gear;
        public string _pinLocation;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public List<string> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public int Obometer
        {
            get => _obometer;
            set => SetProperty(ref _obometer, value);
        }

        public CarTransmission Gear
        {
            get => _gear;
            set => SetProperty(ref _gear, value);
        }

        public string PinLocation
        {
            get => _pinLocation;
            set => SetProperty(ref _pinLocation, value);
        }

        public string CarId
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
                LoadCarId(value);
            }
        }

        public async void LoadCarId(string carId)
        {
            try
            {
                Car car = await this.CarDataStore.GetCarAsync(carId);
                Id = car.Id;
                PinLocation = car.PinLocation;
                Gear = car.Gear;
                Price = car.Price;
                Obometer = car.Obometer;
                Name = car.Name;
                Images = car.Images;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("Failed to Load Car");
            }
        }
    }
}
