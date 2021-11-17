using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TidRod.Services.Interface
{
    public interface IUserDataStore<T>
    {
        Task<bool> AddUserAsync(T item);

        Task<bool> UpdateUserAsync(T item);

        Task<bool> DeleteUserAsync(string id);

        Task<T> GetUserAsync(string id);

        Task<IEnumerable<T>> GetUserCarsAsync(string id);

        Task<IEnumerable<T>> GetUsersAsync(bool forceRefresh = false);
    }
}
