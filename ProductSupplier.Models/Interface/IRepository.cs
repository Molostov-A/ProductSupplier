using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace ProductSupplier.Models.Interface
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task AddAsync(T item);
        Task DeleteAsync(T item);
        Task UpdateAsync(Guid idOldItem, T newItem);
        Task UpdateAsync(string idOldItem, T newItem);
        Task<T> FindAsync(Guid id);
        Task<T> FindAsync(string id);
    }
}