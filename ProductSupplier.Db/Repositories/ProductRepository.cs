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
        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Products.Include(p => p.Categories).ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product item)
        {
            _db.Products.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid idOldProduct, Product newProduct)
        {
            var valuesList = await _db.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(c => c.Id == idOldProduct);

            _db.Products.Remove(valuesList);
            var idCategories = new List<Guid>();
            foreach (var category in newProduct.Categories)
            {
                idCategories.Add(category.Id);
            }

            newProduct.Categories = new List<Category>();
            foreach (var id in idCategories)
            {
                newProduct.Categories.Add(await _db.Categories.FindAsync(id));
            }
            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(string idOldProduct, Product newProduct)
        {
            var Id = Guid.Parse(idOldProduct);
            await UpdateAsync(Id, newProduct);
        }

        public async Task<Product> FindAsync(Guid id)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> FindAsync(string id)
        {
            var Id = Guid.Parse(id);
            return await FindAsync(Id);
        }
    }
}