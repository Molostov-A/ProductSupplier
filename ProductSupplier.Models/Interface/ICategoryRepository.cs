using System.Collections.Generic;
using System;

namespace ProductSupplier.Models.Interface
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Get(int id);
        void Add(Category category);
        void Delete(Category category);
        void Update(Guid idOldCategory, Category newCategory);
        Product Find(Guid id);
    }
}