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
        private readonly List<User> items;

        public MockUserDataStore()
        {
            items =
                new List<User>()
                {
                     new User {
                         Id = "abcd-abcd",
                         Image = "https://api5.iloveimg.com/v1/download/fn17s3thq3fvcsl53yqAkm93czy2dfsd419nhm1c2zbrrh9ml1s6kpw5v2b0y7zkkwnz5w5w86rg30bpsfy88v1yndvw87tvv14nAyjr98456y0m8sbny3A4yscjcmq9t6smd5nj2w9j3sr6tA1y5gdh51rrtwfrnycfdmtp7r0lwptt7Ag1",
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
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateUserAsync(User item)
        {
            var oldUser =
                items.Where((User arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldUser);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var oldUser =
                items.Where((User arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldUser);

            return await Task.FromResult(true);
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<User>>
        GetUsersAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Car>> GetUserCarsAsync(string id)
        {
            ICarDataStore<Car> CarDataStore = DependencyService.Get<ICarDataStore<Car>>();
            var cars = await CarDataStore.GetCarsAsync();
            return await Task.FromResult(cars.Where(car => car.UserId == id).ToList<Car>());
        }
    }
}
