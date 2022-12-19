using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductSupplier.Models;
using ProductSupplier.Models.Interface;

namespace ProductSupplier.Db.Repositories
{
    public class CategoryRepository: IRepository<Category>
    {
        private readonly DatabaseContext _db;
        public CategoryRepository(DatabaseContext databaseContext)
        {
            _db = databaseContext;
        }
        
        public async Task<List<Category>> GetAllAsync()
        {
            var valuesList = await _db.Categories.Include(p => p.Products).ToListAsync();
            return valuesList;
        }

        public async Task AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid idOldCategory, Category newCategory)
        {
            var valuesList = await _db.Categories
                .Include(p => p.Products)
                .FirstOrDefaultAsync(c => c.Id == idOldCategory);

            _db.Categories.Remove(valuesList);
            await _db.SaveChangesAsync();
            var idProducts = new List<Guid>();
            foreach (var product in newCategory.Products)
            {
                idProducts.Add(product.Id);
            }

            newCategory.Products = new List<Product>();
            foreach (var id in idProducts)
            {
                newCategory.Products.Add(await _db.Products.FindAsync(id));
            }
            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(string idOldCategory, Category newCategory)
        {
            var Id = Guid.Parse(idOldCategory);
            await UpdateAsync(Id, newCategory);
        }

        public async Task<Category> FindAsync(Guid id)
        {
            return await _db.Categories.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Category> FindAsync(string id)
        {
            var Id = Guid.Parse(id);
            return await FindAsync(Id);
        }
    }
}