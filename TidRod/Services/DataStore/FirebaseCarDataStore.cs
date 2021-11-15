using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Interface;

namespace TidRod.Services.DataStore
{
    public class FirebaseCarDataStore : IDataStore<Car>
    {
        private readonly FirebaseClient firebase = new FirebaseClient("https://tidrod-7aa6f-default-rtdb.asia-southeast1.firebasedatabase.app/");
        private readonly string DatabaseTableName = "Cars";

        public async Task<bool> AddItemAsync(Car car)
        {
            _ = await firebase.Child(DatabaseTableName).PostAsync(car);
            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            FirebaseObject<Car> toDeletePerson = (await firebase.Child(DatabaseTableName).OnceAsync<Car>()).FirstOrDefault(a => a.Object.Id == id);
            await firebase.Child(DatabaseTableName).Child(toDeletePerson.Key).DeleteAsync();
            return true;
        }

        public async Task<Car> GetItemAsync(string id)
        {
            IEnumerable<Car> item = await GetItemsAsync();
            return item.FirstOrDefault(a => a.Id == id);
        }

        public async Task<IEnumerable<Car>> GetItemsAsync(bool forceRefresh = false)
        {
            //return (await firebase
            //  .Child(DatabaseTableName)
            //  .OnceAsync<Car>()).Select(car => new Car
            //  {
            //      Id = car.Object.Id,
            //      Text = car.Object.Text,
            //      Description = car.Object.Description,
            //      CreateDate = car.Object.CreateDate,
            //      ImageFiles = car.Object.ImageFiles
            //  }).ToList();
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(Car car)
        {
            FirebaseObject<Car> toUpdatePerson = (await firebase.Child(DatabaseTableName).OnceAsync<Car>()).FirstOrDefault(a => a.Object.Id == car.Id);

            await firebase
              .Child(DatabaseTableName)
              .Child(toUpdatePerson.Key)
              .PutAsync(car);
            return true;
        }
    }
}