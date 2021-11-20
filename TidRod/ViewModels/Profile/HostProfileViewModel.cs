using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TidRod.Models;
using Xamarin.Forms;
using System.Linq;

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
        private List<Car> _userCarsList;

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

        public List<Car> UserCarList
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
                if (car == null)
                {
                    Console.WriteLine("No car");
                    return;
                }
                // get user data
                User user = await UserDataStore.GetUserAsync(car.UserId);

                if (user == null)
                {
                    Console.WriteLine("NO user ");
                    return;
                }

                Id = car.Id;
                Name = car.Name;
                Obometer = car.Obometer;
                Gear = car.Gear;
                Images = car.Images;
                user.Image = string.IsNullOrEmpty(user.Image) ? "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg" : user.Image;
                user.Phone = string.Format("{0:### ### ###}", Convert.ToInt64(user.Phone));
                Host = user;
                UserCarList = (await UserDataStore.GetUserCarsAsync(user.Id)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);

                Debug.WriteLine("Failed to Load Car");
            }
        }
    }
}
