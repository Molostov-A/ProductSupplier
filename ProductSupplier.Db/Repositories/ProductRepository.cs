using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductSupplier.Models;
using ProductSupplier.Models.Interface;

namespace ProductSupplier.Db.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly DatabaseContext _databaseContext;
        public ProductRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Product> GetAll()
        {
            var value =  _databaseContext.Products.Include(p => p.Categories).ToList();
            return value;
        }

        public Product Get(Guid id)
        {
            var product = _databaseContext.Products.FirstOrDefault(p => p.Id == id);
            return product;
        }

        public void Add(Product product)
        {
            _databaseContext.Products.Add(product);
            _databaseContext.SaveChanges();
        }

        public void Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Guid idOldProduct, Product newProduct)
        {
            throw new NotImplementedException();
        }

        public Product Find(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}