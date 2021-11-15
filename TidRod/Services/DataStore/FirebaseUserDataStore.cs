using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Interface;

namespace TidRod.Services.DataStore
{
    public class FirebaseUserDataStore : IDataStore<User>
    {
        private readonly FirebaseClient firebase = new FirebaseClient("https://tidrod-7aa6f-default-rtdb.asia-southeast1.firebasedatabase.app/");
        private readonly string DatabaseTableName = "Users";

        public async Task<bool> AddItemAsync(User user)
        {
            _ = await firebase.Child(DatabaseTableName).PostAsync(user);
            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            FirebaseObject<User> toDeletePerson = (await firebase.Child(DatabaseTableName).OnceAsync<User>()).FirstOrDefault(a => a.Object.Id == id);
            await firebase.Child(DatabaseTableName).Child(toDeletePerson.Key).DeleteAsync();
            return true;
        }

        public async Task<User> GetItemAsync(string id)
        {
            IEnumerable<User> item = await GetItemsAsync();
            return item.FirstOrDefault(a => a.Id == id);
        }

        public async Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false)
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

        public async Task<bool> UpdateItemAsync(User user)
        {
            FirebaseObject<User> toUpdatePerson = (await firebase.Child(DatabaseTableName).OnceAsync<User>()).FirstOrDefault(a => a.Object.Id == user.Id);

            await firebase
              .Child(DatabaseTableName)
              .Child(toUpdatePerson.Key)
              .PutAsync(user);
            return true;
        }
    }
}