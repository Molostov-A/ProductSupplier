using System.Collections.Generic;
using System;

namespace ProductSupplier.Models.Interface
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        void Add(T item);
        void Delete(T item);
        void Update(Guid idOldItem, T newItem);
        void Update(string idOldItem, T newItem);
        T Find(Guid id);
        T Find(string id);
    }
}