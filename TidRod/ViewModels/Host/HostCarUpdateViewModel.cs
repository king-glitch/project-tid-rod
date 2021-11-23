﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Helper;
using TidRod.Views.Host;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.ViewModels.Host
{
    [QueryProperty(nameof(CarId), nameof(CarId))]
    [QueryProperty(nameof(PinLocation), nameof(PinLocation))]
    public class HostCarUpdateViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string _name;
        public int _price;
        public int _obometer;
        public string _pinLocation;

        private string _priceError;
        private string _obometerError;
        private string _nameError;
        private string _yourlocationLabel;
        public CarTransmission _gear;
        public List<FileImage> _images;
        public List<string> Gears;
        public Command UpdateHostCommand { get; }
        public Command PinLocationCommand { get; }
        public Command ChooseImageCommand { get; }
        private readonly FSHelper helper = new FSHelper();
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

        public string YourLocationLabel
        {
            get => _yourlocationLabel;
            set => SetProperty(ref _yourlocationLabel, value);
        }

        public string PriceError
        {
            get => _priceError;
            set => SetProperty(ref _priceError, value);
        }

        public string ObometerError
        {
            get => _obometerError;
            set => SetProperty(ref _obometerError, value);
        }

        public string NameError
        {
            get => _nameError;
            set => SetProperty(ref _nameError, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public List<FileImage> Images
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
            set
            {
                SetYourLocationLabel(value);
                SetProperty(ref _pinLocation, value);
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

                foreach (var image in car.Images)
                {
                    Images.Add(new FileImage
                    {
                        FileName = "-",
                        FileURL = image,
                        Image = null
                    });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("Failed to Load Car");
            }
        }

        public HostCarUpdateViewModel()
        {
            UpdateHostCommand = new Command(OnUpdate);
            PinLocationCommand = new Command(OnPinLocation);
            ChooseImageCommand = new Command(OnChooseImage);
            Images = new List<FileImage>();
            Gears = new List<string> { "Manual", "Automatic" };
        }

        private void ResetForms()
        {
            PriceError = "";
            ObometerError = "";
            NameError = "";
        }

        private async void SetYourLocationLabel(string value)
        {
            string address;
            try
            {
                // call geo code
                Geocoder geoCoder = new Geocoder();

                //split lan, long
                var pos = value.Split(',');

                // get position
                Position position =
                    new Position(float.Parse(pos[0]), float.Parse(pos[1]));

                // get possible addresses
                IEnumerable<string> possibleAddresses =
                    await geoCoder.GetAddressesForPositionAsync(position);

                // get first address
                address = possibleAddresses.FirstOrDefault();
            }
            catch (Exception ex)
            {
                address = MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC;

            }
            YourLocationLabel = address;
        }

        public async void OnUpdate()
        {
            // reset all forms
            this.ResetForms();

            // if busy return;
            if (this.IsBusy)
            {
                return;
            }

            // error validations
            bool IsError = false;

            if (Price <= 0)
            {
                PriceError = MainLanguage.HOST_PRICE_EMPTY;
                IsError = true;
            }


            if (Obometer <= 0)
            {
                ObometerError = MainLanguage.HOST_OBOMETER_EMPTY;
                IsError = true;
            }

            if (string.IsNullOrEmpty(Name))
            {
                NameError = MainLanguage.HOST_NAME_EMPTY;
                IsError = true;
            }

            if (Images.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE,
                    MainLanguage.HOST_IMAGE_EMPTY, "OK"); ;
                IsError = true;
            }

            if (string.IsNullOrEmpty(PinLocation))
            {
                await Application.Current.MainPage.DisplayAlert(
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE,
                    MainLanguage.HOST_PIN_EMPTY, "OK"); ;
                IsError = true;
            }

            if (IsError)
            {
                return;
            }
            this.IsBusy = true;
            if (Images != Images)
            {
                List<string> Images = await CheckAndSaveImage();
            }


            await CarDataStore.AddCarAsync(new Car()
            {
                Id = Guid.NewGuid().ToString(),
                Gear = Gear,
                UserId = App.CurrentSession,
                Name = Name,
                Obometer = Obometer,
                PinLocation = PinLocation,
                Price = Price,
                //Images = Images.SelectMany<string>(i => i.FileURL)
            }) ;

            Shell.Current.GoToAsync("..");

            this.IsBusy = false;
        }

        private async Task<List<string>> CheckAndSaveImage()
        {
            List<string> URLString = new List<string>();
            if (Images?.Count > 0)
            {
                foreach (var _file in Images)
                {
                    URLString.Add(await SaveFileToServer(_file));
                }
            }
            return URLString;
        }

        private async Task<string> SaveFileToServer(FileImage _file)
        {
            string _uriFile = "";
            var imageAsBytes = ImageSourceToBytes(_file.Image);
            if (imageAsBytes != null)
            {
                using (var StreamF = new MemoryStream(imageAsBytes))
                {
                    _uriFile = await helper.UploadFile(StreamF, _file.FileName);
                }
            }
            return _uriFile;
        }

        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            var cancellationToken = CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            byte[] bytesAvailable = new byte[stream.Length];
            stream.Read(bytesAvailable, 0, bytesAvailable.Length);
            return bytesAvailable;
        }

        public async void OnPinLocation()
        {
            await Shell.Current.GoToAsync($"{nameof(HostPinLocationPage)}?{nameof(HostPinLocationViewModel.PinLocation)}={PinLocation}");
        }

        public async void OnChooseImage(object obj)
        {
            IEnumerable<FileResult> images = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick Image(s)"
            });

            foreach (var image in images)
            {

                if (image != null)
                {

                    Stream stream = await image.OpenReadAsync();

                    byte[] _imageByte = StreamToByteArray(stream);

                    Images.Add(new FileImage
                    {
                        FileName = image.FileName,
                        FileURL = "-",
                        Image = ImageSource.FromStream(() => new MemoryStream(_imageByte))
                    });

                }
            }

            try
            {
                var target = (CarouselView)obj;
                target.ItemsSource = Images;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
        private byte[] StreamToByteArray(Stream source)
        {
            byte[] imageAsBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                source.CopyTo(memoryStream);
                imageAsBytes = memoryStream.ToArray();
            }

            return imageAsBytes;
        }
    }
}