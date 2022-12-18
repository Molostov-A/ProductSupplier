using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public List<Category> GetAll()
        {
            var valuesList = _db.Categories.Include(p => p.Products).ToList();
            return valuesList;
        }

        public void Add(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        public void Delete(Category category)
        {
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }

        public void Update(Guid idOldCategory, Category newCategory)
        {
            var valuesList = _db.Categories
                .Include(p => p.Products)
                .FirstOrDefault(c => c.Id == idOldCategory);

            _db.Categories.Remove(valuesList);
            var idProducts = new List<Guid>();
            foreach (var product in newCategory.Products)
            {
                idProducts.Add(product.Id);
            }

            newCategory.Products = new List<Product>();
            foreach (var id in idProducts)
            {
                newCategory.Products.Add(_db.Products.Find(id));
            }
            _db.Categories.Add(newCategory);
            _db.SaveChanges();
        }

        public void Update(string idOldCategory, Category newCategory)
        {
            var Id = Guid.Parse(idOldCategory);
            Update(Id, newCategory);
        }

        public Category Find(Guid id)
        {
            var item = _db.Categories.FirstOrDefault(p => p.Id == id);
            return item;
        }
        public Category Find(string id)
        {
            var Id = Guid.Parse(id);
            return Find(Id);
        }
    }
}