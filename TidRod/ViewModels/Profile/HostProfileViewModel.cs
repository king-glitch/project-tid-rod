using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TidRod.Models;
using Xamarin.Forms;

namespace TidRod.ViewModels.Profile
{
    [QueryProperty(nameof(CarId), nameof(CarId))]
    public class HostProfileViewModel : BaseViewModel
    {
        private string Id { get; set; }
        private string _name;
        private int _obometer;
        private CarTransmission _gear;
        private List<string> _images;
        private User _host;
        private IEnumerable<Car> _userCarsList;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public CarTransmission Gear
        {
            get => _gear;
            set => SetProperty(ref _gear, value);
        }

        public List<string> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public int Obometer
        {
            get => _obometer;
            set => SetProperty(ref _obometer, value);
        }

        public User Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }

        public IEnumerable<Car> UserCarList
        {
            get => _userCarsList;
            set => SetProperty(ref _userCarsList, value);
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

                // get car data
                Car car = await CarDataStore.GetCarAsync(carId);

                // get user data
                User user = await UserDataStore.GetUserAsync(car.UserId);

                if (car == null || user == null)
                {
                    return;
                }

                Id = car.Id;
                Name = car.Name;
                Obometer = car.Obometer;
                Gear = car.Gear;
                Images = car.Images;
                user.Phone = string.Format("{0:### ### ###}", Convert.ToInt64(user.Phone));
                Host = user;
                UserCarList = await UserDataStore.GetUserCarsAsync(user.Id);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Car");
            }
        }
    }
}
