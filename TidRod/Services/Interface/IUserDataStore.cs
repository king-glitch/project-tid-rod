using System.Collections.Generic;
using System.Threading.Tasks;
using TidRod.Models;

namespace TidRod.Services.Interface
{
    public interface IUserDataStore<T>
    {
        Task<bool> AddUserAsync(T item);

        Task<T> UpdateUserAsync(T item);

        Task<bool> DeleteUserAsync(string id);

        Task<T> GetUserAsync(string id);

        Task<IEnumerable<Car>> GetUserCarsAsync(string id);

        Task<IEnumerable<User>> GetUsersAsync(bool forceRefresh = false);
    }
}
