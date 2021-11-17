using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using TidRod.Models;
using TidRod.Services.Interface;

namespace TidRod.Services.DataStore.Firebase
{
    public class FirebaseCarDataStore : ICarDataStore<Car>
    {
        private readonly FirebaseClient
            firebase =
                new FirebaseClient("https://tidrod-7aa6f-default-rtdb.asia-southeast1.firebasedatabase.app/");

        private readonly string DatabaseTableName = "Cars";

        public async Task<bool> AddCarAsync(Car car)
        {
            _ = await firebase.Child(DatabaseTableName).PostAsync(car);
            return true;
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            FirebaseObject<Car> toDeletePerson =
                (await firebase.Child(DatabaseTableName).OnceAsync<Car>())
                    .FirstOrDefault(a => a.Object.Id == id);
            await firebase
                .Child(DatabaseTableName)
                .Child(toDeletePerson.Key)
                .DeleteAsync();
            return true;
        }

        public async Task<Car> GetCarAsync(string id)
        {
            IEnumerable<Car> item = await GetCarsAsync();
            return item.FirstOrDefault(a => a.Id == id);
        }

        public async Task<IEnumerable<Car>>
        GetCarsAsync(bool forceRefresh = false)
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

        public async Task<bool> UpdateCarAsync(Car car)
        {
            FirebaseObject<Car> toUpdatePerson =
                (await firebase.Child(DatabaseTableName).OnceAsync<Car>())
                    .FirstOrDefault(a => a.Object.Id == car.Id);

            await firebase
                .Child(DatabaseTableName)
                .Child(toUpdatePerson.Key)
                .PutAsync(car);
            return true;
        }
    }
}
