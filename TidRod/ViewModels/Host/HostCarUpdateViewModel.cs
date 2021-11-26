using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Utils;
using TidRod.Views.Host;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.ViewModels.Host
{
    [QueryProperty(nameof(CarId), nameof(CarId))]
    [QueryProperty(nameof(PinLocation), nameof(PinLocation))]
    [QueryProperty(nameof(ReInitialize), nameof(ReInitialize))]
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
        private int _reInitialize;
        public CarTransmission _gear;
        public int _gearInt;
        public List<FileImage> _images;
        public List<string> CarGears { get; set; }
        public Command UpdateHostCommand { get; }
        public Command PinLocationCommand { get; }
        public Command ChooseImageCommand { get; }
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

        public int ReInitialize
        {
            get => _reInitialize;
            set => SetProperty(ref _reInitialize, value);
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

        public int GearInt
        {
            get => _gearInt;
            set => SetProperty(ref _gearInt, value);
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
                // delay because some task are not done; (just to make sure);
                await Task.Delay(500);

                // check if shoud reinitialize;
                if (ReInitialize == 1)
                {
                    return;
                }

                // get the car infomation;
                Car car = await this.CarDataStore.GetCarAsync(carId);

                // check if car is valid;
                if (car == null)
                {
                    // if not then show the error message;
                    await Shell.Current.GoToAsync($"..");
                    await Application.Current.MainPage.DisplayAlert(
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE,
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC, "OK"); ;
                }

                // save the info in variable, to show to the user;

                PinLocation = car.PinLocation;
                Gear = car.Gear;
                Price = car.Price;
                Obometer = car.Obometer;
                Name = car.Name;
                GearInt = car.Gear == CarTransmission.Automatic ? 0 : 1;

                // temp images;
                var tempImages = new List<FileImage>();

                // save all images in temp images;
                foreach (var image in car.Images)
                {
                    tempImages.Add(new FileImage
                    {
                        FileName = "-",
                        FileURL = image,
                        Image = null
                    });
                }
                // then save the temp images to the main variable;
                Images = tempImages;
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
            CarGears = new List<string> { "Automatic", "Manual" };
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
            catch
            {
                // show the error
                // this errors mostly presist on the phone internet
                // try to reinitialize the phone internet;
                // this error are the phone problem, not mine;

                // error message (grfc something)
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

            // let the app be busy;
            this.IsBusy = true;

            // find the current car data;
            Car car = await this.CarDataStore.GetCarAsync(Id);

            // convert images data to string;
            List<string> stringImages = Images.Select(i => i.FileURL).ToList();

            // if images is not the same, then update it.
            if (Images.Where(i => i.Image != null).Count() > 0)
            {
                try
                {
                    car.Images = await CheckAndSaveImage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            // update the new data

            car.Gear = Gear;
            car.Name = Name;
            car.PinLocation = PinLocation;
            car.Obometer = Obometer;
            car.Price = Price;


            // push to database;
            await CarDataStore.UpdateCarAsync(car);


            // go back to previous page
            await Shell.Current.GoToAsync("..");


            // let the app be free.
            this.IsBusy = false;
        }

        private async Task<List<string>> CheckAndSaveImage()
        {
            List<string> URLString = new List<string>();
            if (Images?.Count == 0)
            {
                return null;
            }

            foreach (var _file in Images)
            {
                if (_file.Image == null)
                {
                    continue;
                }

                URLString.Add(await TidRodUtilitiles.SaveFileToServer(_file, AppSettings.FIREBASE_STORAGE_ROOT_CAR));
            }

            return URLString;
        }


        public async void OnPinLocation()
        {
            await Shell.Current.GoToAsync($"{nameof(HostPinLocationPage)}?{nameof(HostPinLocationViewModel.PinLocation)}={PinLocation}&{nameof(HostPinLocationViewModel.CarId)}={CarId}");
        }

        public async void OnChooseImage(object obj)
        {
            try
            {


                // if images variable is null, then initialize the Images collection;

                Images = new List<FileImage>();
                // select the image(s)

                IEnumerable<FileResult> images = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick Image(s)"
                });
                // check if image are selected;

                if (images == null || images.Count() == 0)
                {
                    return;
                }

                // run all images;

                foreach (var image in images)
                {
                    // check if image are correct;

                    if (image == null)
                    {
                        continue;
                    }
                    // then start the steam the image in to bytes;

                    Stream stream = await image.OpenReadAsync();
                    // convert stream into byte array;

                    byte[] _imageByte = TidRodUtilitiles.StreamToByteArray(stream);
                    // add image to the collection;

                    Images.Add(new FileImage
                    {
                        FileName = image.FileName,
                        FileURL = "-",
                        Image = ImageSource.FromStream(() => new MemoryStream(_imageByte))
                    });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            try
            {
                // show the images to the user;
                // this doesn't work;

                // the image are in different formats;
                // I don't know how to change this;
                var target = (CarouselView)obj;
                target.ItemsSource = Images;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
}
