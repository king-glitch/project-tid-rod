using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Helper;
using TidRod.Utils;
using TidRod.Views.Host;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TidRod.ViewModels.Host
{
    [QueryProperty(nameof(PinLocation), nameof(PinLocation))]
    public class HostCarAddViewModel : BaseViewModel
    {
        public string _name;
        public int _price;
        public int _obometer;
        public string _pinLocation;

        private string _priceError;
        private string _obometerError;
        private string _nameError;
        private string _yourlocationLabel;
        public CarTransmission _gear;
        public ObservableCollection<FileImage> _images;
        public List<string> CarGears { get; set; }
        public Command AddHostCommand { get; }
        public Command PinLocationCommand { get; }
        public Command ChooseImageCommand { get; }
        private readonly FSHelper helper = new FSHelper();


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

        public ObservableCollection<FileImage> Images
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

        public HostCarAddViewModel()
        {
            AddHostCommand = new Command(OnAddHost);
            PinLocationCommand = new Command(OnPinLocation);
            ChooseImageCommand = new Command(OnChooseImage);
            Images = new ObservableCollection<FileImage>();
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
                var pos = value?.Split(',');

                // get position
                Position position =
                    new Position(float.Parse(pos[0]), float.Parse(pos[1]));

                // get possible addresses
                IEnumerable<string> possibleAddresses =
                    await geoCoder?.GetAddressesForPositionAsync(position);

                // get first address
                address = possibleAddresses.FirstOrDefault();
            }
            catch
            {
                address = MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC;

            }
            YourLocationLabel = address;
        }

        public async void OnAddHost()
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

            // check if price are correct;
            if (Price <= 0)
            {
                PriceError = MainLanguage.HOST_PRICE_EMPTY;
                IsError = true;
            }

            // check if obometer are correct;

            if (Obometer <= 0)
            {
                ObometerError = MainLanguage.HOST_OBOMETER_EMPTY;
                IsError = true;
            }

            // check if name are empty;

            if (string.IsNullOrEmpty(Name))
            {
                NameError = MainLanguage.HOST_NAME_EMPTY;
                IsError = true;
            }

            // if image is exists;

            if (Images.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE,
                    MainLanguage.HOST_IMAGE_EMPTY, "OK"); ;
                IsError = true;
            }

            // if pin are pinned;

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
            // save the image to the database;
            Task<List<string>> TaskA = Task.Run(async () => await CheckAndSaveImage());
            // save the car info to the database;
            await TaskA?.ContinueWith(antecedent => { _ = CarDataStore.AddCarAsync(new Car() { Id = Guid.NewGuid().ToString(), Gear = Gear, UserId = App.CurrentSession, Name = Name, Obometer = Obometer, PinLocation = PinLocation, Price = Price, Images = antecedent.Result }); Shell.Current.GoToAsync(".."); }, TaskContinuationOptions.OnlyOnRanToCompletion);
            // release the busy task;
            this.IsBusy = false;
        }

        private async Task<List<string>> CheckAndSaveImage()
        {
            // create new urls;
            List<string> URLString = new List<string>();

            // check if images are exist;
            if (Images?.Count > 0)
            {
                // run all images;
                foreach (var _file in Images)
                {
                    // add it the the list
                    URLString?.Add(await TidRodUtilitiles.SaveFileToServer(_file, AppSettings.FIREBASE_STORAGE_ROOT_CAR));
                }
            }
            // return all images;
            return URLString;
        }

        public async void OnPinLocation()
        {
            // go to pin page;
            await Shell.Current.GoToAsync($"{nameof(HostPinLocationPage)}");
        }

        public async void OnChooseImage(object obj)
        {
            try
            {

                // if images variable is null, then initialize the Images collection;
                if (Images == null)
                {
                    Images = new ObservableCollection<FileImage>();
                }

                // select the image(s)

                IEnumerable<FileResult> images = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick Image(s)"
                });

                // check if image are selected;

                if (images == null || images?.Count() == 0)
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

                    Stream stream = await image?.OpenReadAsync();

                    // convert stream into byte array;
                    byte[] _imageByte = TidRodUtilitiles.StreamToByteArray(stream);

                    // add image to the collection;
                    Images?.Add(new FileImage { 
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

                // show the new image to the user;
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
