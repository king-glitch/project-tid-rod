using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Helper;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TidRod.ViewModels.Profile
{
    public class UserProfileSettingsViewModel : BaseViewModel
    {
        public Command UpdateUserProfile { get; }
        public Command ChangeUserImage { get; }
        private readonly FSHelper fSHelper = new FSHelper();
        private string _firstName;
        private string _lastName;
        private string _email;

        private string _firstNameError;
        private string _lastNameError;
        private string _emailError;
        private FileImage _image;
        private User _user;

        public User CurrentUser
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }
        public string FirstNameError
        {
            get => _firstNameError;
            set => SetProperty(ref _firstNameError, value);
        }
        public string LastNameError
        {
            get => _lastNameError;
            set => SetProperty(ref _lastNameError, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public FileImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private void ResetForms()
        {
            EmailError = "";
            FirstNameError = "";
            LastNameError = "";
        }

        public UserProfileSettingsViewModel()
        {
            UpdateUserProfile = new Command(OnUpdate);
            ChangeUserImage = new Command(OnChangeUserImage);
        }

        public async void OnChangeUserImage(object obj)
        {

            FileResult image = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick Profile Image"
            });

            if (image != null)
            {
                Stream stream = await image.OpenReadAsync();

                byte[] _imageByte = StreamToByteArray(stream);
                Image = new FileImage
                {
                    FileName = image.FileName,
                    FileURL = "-",
                    Image = ImageSource.FromStream(() => new MemoryStream(_imageByte))
                };

                try
                {
                    var target = (Image)obj;
                    target.Source = Image.Image;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }


            }

        }

        private async Task<string> CheckAndSaveImage(FileImage image)
        {
            if (image != null)
            {
                return await SaveFileToServer(image);
            }
            return null;
        }

        private async Task<string> SaveFileToServer(FileImage _file)
        {
            string _uriFile = "";
            var imageAsBytes = ImageSourceToBytes(_file.Image);
            if (imageAsBytes != null)
            {
                using (var StreamF = new MemoryStream(imageAsBytes))
                {
                    _uriFile = await this.fSHelper.UploadFile(StreamF, _file.FileName);
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

        public async void OnAppearing()
        {
            CurrentUser = await this.UserDataStore.GetUserAsync(App.CurrentSession);

            Email = CurrentUser.Email;
            FirstName = CurrentUser.FirstName;
            LastName = CurrentUser.LastName;
            Image = CurrentUser.Image;
        }

        public async void OnUpdate()
        {

            // reset all errors;
            this.ResetForms();

            // get user current info;
            var user = CurrentUser;

            // check if email already existed and not same as the current one;

            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE,
                    MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC, "Fuck");
                return;
            }

            bool IsError = false;
            // check if email null;
            if (string.IsNullOrEmpty(Email))
            {
                EmailError = MainLanguage.PROFILE_EMAIL_EMPTY;
                IsError = true;
            }
            else if (
                  Email != user.Email && !this.IsValidEmail(Email))
            {
                // show invalid error;
                EmailError = MainLanguage.PROFILE_EMAIL_INVALID;
                IsError = true;
            }
            else
            {
                var users = await this.UserDataStore.GetUsersAsync();
                if (Email != user.Email && users.ToList().Where(u => u.Email.ToLower() == Email.ToLower()).Count() > 0)
                {
                    // show already exist email;
                    EmailError = MainLanguage.PROFILE_EMAIL_EXISTS;
                    IsError = true;
                }
            }

            // check if first name null;
            if (string.IsNullOrEmpty(FirstName))
            {
                FirstNameError = MainLanguage.PROFILE_FIRST_NAME_EMPTY;
                IsError = true;
            }


            // check if last name null;
            if (string.IsNullOrEmpty(LastName))
            {
                LastNameError = MainLanguage.PROFILE_LAST_NAME_EMPTY;
                IsError = true;
            }


            // check all errors if presist;

            if (IsError)
            {
                return;
            }


            // try to update;
            try
            {

                // update user data;

                user.FirstName = FirstName;
                user.LastName = LastName;
                user.Email = Email;

                // check if image is changed

                if (Image != null && Image != user.Image)
                {
                    Image.FileURL = await CheckAndSaveImage(Image);
                    user.Image = Image;
                }

                CurrentUser = await this.UserDataStore.UpdateUserAsync(user);
                await Application.Current.MainPage.DisplayAlert(
                        MainLanguage.GENERAL_SUCCESSFULLY_TITLE,
                        MainLanguage.PROFILE_UPDATE_SUCCESSFULLY, "yeah");
                await Shell.Current.GoToAsync("..");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            // something went wrong;
            await Application.Current.MainPage.DisplayAlert(MainLanguage.GENERAL_SOMETHING_WENT_WRONG_TITLE, MainLanguage.GENERAL_SOMETHING_WENT_WRONG_DESC, "Fuck");
            return;
        }

        private bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
