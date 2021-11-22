using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Interface;

namespace TidRod.Services.DataStore.Firebase
{
    public class FirebaseUserDataStore : IUserDataStore<User>
    {
        private readonly FirebaseClient firebase = new FirebaseClient(AppSettings.FIREBASE_DATABASE_URL);
        private readonly string DatabaseTableName = AppSettings.FIREBASE_DATABASE_USER_ROOT;

        public async Task<bool> AddUserAsync(User user)
        {
            _ = await firebase.Child(DatabaseTableName).PostAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            FirebaseObject<User> toDeletePerson = (await firebase.Child(DatabaseTableName).OnceAsync<User>()).FirstOrDefault(a => a.Object.Id == id);
            await firebase.Child(DatabaseTableName).Child(toDeletePerson.Key).DeleteAsync();
            return true;
        }

        public async Task<User> GetUserAsync(string id)
        {
            IEnumerable<User> item = await GetUsersAsync();
            return item.FirstOrDefault(a => a.Id == id);
        }

        public Task<IEnumerable<Car>> GetUserCarsAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(bool forceRefresh = false)
        {
            throw new System.NotImplementedException();
            //return (await firebase
            //  .Child(DatabaseTableName)
            //  .OnceAsync<User>()).Select(user => new User
            //  {
            //      Id = user.Object.Id,
            //      Text = user.Object.Text,
            //      Description = user.Object.Description,
            //      CreateDate = user.Object.CreateDate,
            //      ImageFiles = user.Object.ImageFiles
            //  }).ToList();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            FirebaseObject<User> toUpdatePerson = (await firebase.Child(DatabaseTableName).OnceAsync<User>()).FirstOrDefault(a => a.Object.Id == user.Id);

            await firebase
              .Child(DatabaseTableName)
              .Child(toUpdatePerson.Key)
              .PutAsync(user);


            return await this.GetUserAsync(user.Id);

        }
    }
}