using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Interface;
using Xamarin.Forms;

namespace TidRod.Services.DataStore.Mock
{
    public class MockUserDataStore : IUserDataStore<User>
    {
        private readonly List<User> users;

        public MockUserDataStore()
        {
            users =
                new List<User>()
                {
                     new User {
                         Id = "abcd-abcd",
                         Image = new FileImage
                         {
                             FileURL = AppSettings.USER_DEFAULT_AVATAR,
                             FileName = "abcd-abcd.avatar.png"
                         },
                         FirstName = "Deku",
                         LastName = "Academia",
                         Email = "deku@academia.ua",
                         Phone = "0990990990",
                         Password = "Password"
                     },
                };
        }

        public async Task<bool> AddUserAsync(User item)
        {
            users.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var oldUser =
                users.Where((User arg) => arg.Id == user.Id).FirstOrDefault();
            users.Remove(oldUser);
            users.Add(user);
            
            if (await Task.FromResult(true))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var oldUser =
                users.Where((User arg) => arg.Id == id).FirstOrDefault();
            users.Remove(oldUser);

            return await Task.FromResult(true);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var data = await Task.FromResult(users.FirstOrDefault(s => s.Id == id));

            if (data == null) return null;

            if (data.Image == null)
            {
                data.Image = new FileImage
                {
                    FileURL = "https://api5.iloveimg.com/v1/download/fn17s3thq3fvcsl53yqAkm93czy2dfsd419nhm1c2zbrrh9ml1s6kpw5v2b0y7zkkwnz5w5w86rg30bpsfy88v1yndvw87tvv14nAyjr98456y0m8sbny3A4yscjcmq9t6smd5nj2w9j3sr6tA1y5gdh51rrtwfrnycfdmtp7r0lwptt7Ag1",
                    FileName = "abcd-abcd.avatar.png"
                };
                await this.UpdateUserAsync(data);
            }
            return data;
        }

        public async Task<IEnumerable<User>>
        GetUsersAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(users);
        }

        public async Task<IEnumerable<Car>> GetUserCarsAsync(string id)
        {
            ICarDataStore<Car> CarDataStore = DependencyService.Get<ICarDataStore<Car>>();
            var cars = await CarDataStore.GetCarsAsync();
            return await Task.FromResult(cars.Where(car => car.UserId == id).ToList<Car>());
        }
    }
}
