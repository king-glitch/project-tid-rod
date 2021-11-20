using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;

namespace TidRod.ViewModels.Profile
{
    public class UserProfileViewModel : BaseViewModel
    {
        private string Id { get; set; }
        private User _user;
        private IEnumerable<Car> _userCarList;


        public bool IsUserDataFound { get; set; }

        public User CurrentSessionUser
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        public IEnumerable<Car> UserCarList
        {
            get => _userCarList;
            set => SetProperty(ref _userCarList, value);
        }

        public string UserId
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
                LoadUserId(value);
            }
        }

        public async void LoadUserId(string userId)
        {
            try
            {

                IsUserDataFound = false;

                // get user data
                User user = await UserDataStore?.GetUserAsync(userId);

                if (user == null)
                {
                    Console.WriteLine("NO User Data");
                    return;
                }

                IsUserDataFound = true;

                var profileImage = string.IsNullOrEmpty(user.Image) ? "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg" : user.Image;

                Console.WriteLine(profileImage);

                try
                {
                    user.Image = profileImage;

                }
                catch
                {
                    user.Image = "https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-image-182145777.jpg";
                }

                try
                {
                    user.Phone = string.Format("{0:### ### ###}", Convert.ToInt64(user.Phone));
                }
                catch
                {
                    user.Phone = user.Phone;
                }

                CurrentSessionUser = user;

                UserCarList = await UserDataStore.GetUserCarsAsync(user.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
