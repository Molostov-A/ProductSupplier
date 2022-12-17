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
            throw new NotImplementedException();
        }

        public Category Find(Guid id)
        {
            var item = _db.Categories.FirstOrDefault(p => p.Id == id);
            return item;
        }
    }
}