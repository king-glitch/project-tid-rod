using System;
using System.Collections.Generic;
using System.Diagnostics;
using TidRod.Models;
using TidRod.Views.General;
using Xamarin.Forms;

namespace TidRod.ViewModels.Profile
{
    public class UserProfileViewModel : BaseViewModel
    {
        private readonly string Id = App.CurrentSession;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _image;
        private IEnumerable<Car> _userCarList;
        public Command UserLogoutCommand { get; }



        public bool IsUserDataFound { get; set; }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public IEnumerable<Car> UserCarList
        {
            get => _userCarList;
            set => SetProperty(ref _userCarList, value);
        }

        public UserProfileViewModel()
        {
            UserLogoutCommand = new Command(OnUserLogout);
        }

        private async void OnUserLogout()
        {
            App.CurrentSession = null;

            await Shell.Current.GoToAsync($"///{nameof(MainPage)}/");
        }

        public void OnAppearing()
        {
            this.LoadUserData();
        }

        public async void LoadUserData()
        {
            try
            {

                IsUserDataFound = false;

                // get user data
                User user = await UserDataStore?.GetUserAsync(Id);

                if (user == null)
                {
                    Console.WriteLine("NO User Data");
                    return;
                }

                IsUserDataFound = true;

                bool IsImageNull = user?.Image?.FileURL == null || string.IsNullOrEmpty(user.Image.FileURL);
                string profileImage = IsImageNull ? AppSettings.USER_DEFAULT_AVATAR : user.Image.FileURL;

                try
                {
                    if (user.Image == null)
                    {
                        user.Image = new FileImage
                        {
                            FileName = "user.avatar.png",
                            FileURL = AppSettings.USER_DEFAULT_AVATAR
                        };
                    }
                    else
                    {

                        user.Image.FileURL = profileImage;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);

                }

                try
                {
                    user.Phone = string.Format("{0:## ### ####}", Convert.ToInt64(user.Phone));
                }
                catch
                {
                    user.Phone = user.Phone;
                }

                FirstName = char.ToUpper(user.FirstName[0]) + user.FirstName.Substring(1);
                LastName = char.ToUpper(user.LastName[0]) + user.LastName.Substring(1);
                Email = user.Email;
                Phone = user.Phone;
                Image = user.Image.FileURL;

                UserCarList = await UserDataStore.GetUserCarsAsync(user.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
