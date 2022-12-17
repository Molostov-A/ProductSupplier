using System;
using System.Collections.Generic;

namespace ProductSupplier.Models.Interface
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product Get(int id);
        void Add(Product product);
        void Delete(Product product);
        void Update(Guid idOldProduct, Product newProduct);
        Product Find (Guid id);
    }
}