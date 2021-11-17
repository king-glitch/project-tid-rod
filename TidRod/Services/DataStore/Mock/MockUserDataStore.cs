using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Interface;

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
                    // new User {
                    //     Id = Guid.NewGuid().ToString(),
                    //     Text = "First item",
                    //     Description = "This is an item description."
                    // },
                    // new User {
                    //     Id = Guid.NewGuid().ToString(),
                    //     Text = "Second item",
                    //     Description = "This is an item description."
                    // },
                    // new User {
                    //     Id = Guid.NewGuid().ToString(),
                    //     Text = "Third item",
                    //     Description = "This is an item description."
                    // },
                    // new User {
                    //     Id = Guid.NewGuid().ToString(),
                    //     Text = "Fourth item",
                    //     Description = "This is an item description."
                    // },
                    // new User {
                    //     Id = Guid.NewGuid().ToString(),
                    //     Text = "Fifth item",
                    //     Description = "This is an item description."
                    // },
                    // new User {
                    //     Id = Guid.NewGuid().ToString(),
                    //     Text = "Sixth item",
                    //     Description = "This is an item description."
                    // }
                };
        }

        public async Task<bool> AddUserAsync(User item)
        {
            items.Add (item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateUserAsync(User item)
        {
            var oldUser =
                items.Where((User arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove (oldUser);
            items.Add (item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var oldUser =
                items.Where((User arg) => arg.Id == id).FirstOrDefault();
            items.Remove (oldUser);

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

        public Task<IEnumerable<User>> GetUserCarsAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
