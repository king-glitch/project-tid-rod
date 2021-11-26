using System.Collections.Generic;
using System.Threading.Tasks;

namespace TidRod.Services.Interface
{
    public interface ICarDataStore<T>
    {
        Task<bool> AddCarAsync(T item);

        Task<bool> UpdateCarAsync(T item);

        Task<bool> DeleteCarAsync(string id);

        Task<T> GetCarAsync(string id);

        Task<IEnumerable<T>> GetCarsAsync(bool forceRefresh = false);
    }
}
