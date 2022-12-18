using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductSupplier.Models;
using ProductSupplier.Models.Interface;

namespace ProductSupplier.Db.Repositories
{
    public class ProductRepository: IRepository<Product>
    {
        private readonly DatabaseContext _db;
        public ProductRepository(DatabaseContext databaseContext)
        {
            _db = databaseContext;
        }
        public List<Product> GetAll()
        {
           
            var value =  _db.Products.Include(p => p.Categories).ToList();
            return value;
        }

        public void Add(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void Delete(Product product)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        public void Update(Guid idOldProduct, Product newProduct)
        {
            var product = _db.Products
                .Include(c => c.Categories)
                .Single(p => p.Id == idOldProduct);
            product = newProduct;
            _db.SaveChanges();
        }

        public void Update(string idOldProduct, Product newProduct)
        {
            var Id = Guid.Parse(idOldProduct);
            Update(Id, newProduct);
        }

        public Product Find(Guid id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            return product;
        }
        public Product Find(string id)
        {
            var Id = Guid.Parse(id);
            return Find(Id);
        }
    }
}