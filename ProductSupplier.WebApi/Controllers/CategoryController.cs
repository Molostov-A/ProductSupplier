using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductSupplier.Models.Interface;
using ProductSupplier.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using ProductSupplier.WebApi.ViewModels;

namespace ProductSupplier.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _repositoryCategory;
        private readonly IRepository<Product> _repositoryProduct;
        private readonly ILogger<CategoryController> _logger;


        public CategoryController(ILogger<CategoryController> logger, IRepository<Category> repositoryCategory, IRepository<Product> repositoryProduct)
        {
            _logger = logger;
            _repositoryCategory = repositoryCategory;
            _repositoryProduct = repositoryProduct;
        }

        //[ScriptIgnore]
        public IEnumerable<Category> GetAll()
        {
            return _repositoryCategory.GetAll();
        }

        [HttpGet("{id}", Name = "Category")]
        public Category GetUnit(string id)
        {
            var Id = Guid.Parse(id);
            var item = _repositoryCategory.Find(Id);
            return item;
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
            {
                return NotFound();
            }
            
            var category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description
            };
            _repositoryCategory.Add(category);
            return new ObjectResult(category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var Id = Guid.Parse(id);
            var value = _repositoryCategory.Find(Id);
            if (value == null)
            {
                return NotFound();
            }
            _repositoryCategory.Delete(value);
            return new ObjectResult(value);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Category item)
        {
            var Id = Guid.Parse(id);
            if (id == null || item.Id != Id)
            {
                return BadRequest();
            }
            var dbItem = _repositoryCategory.Find(id);
            if (dbItem == null)
            {
                return NotFound();
            }
            _repositoryCategory.Update(id, dbItem);
            var dbItemUpdate = _repositoryCategory.Find(id);
            return new ObjectResult(dbItemUpdate);
        }

        [HttpPut("{idCategory}/{idProduct}")]
        public IActionResult AddProductInCategory(string idCategory, string idProduct)
        {
            var category = _repositoryCategory.Find(idCategory);
            if (category == null)
            {
                return NotFound();
            }
            var product = _repositoryProduct.Find(idProduct);
            if (category.Products != null)
            {
                if (category.Products.FirstOrDefault(c => c.Id == product.Id) == null)
                {
                    category.Products.Add(product);
                    _repositoryCategory.Update(idCategory, category);
                }
            }
            else
            {
                category.Products = new List<Product>()
                {
                    product
                };
                _repositoryCategory.Update(idCategory, category);
            }
            return new ObjectResult(_repositoryProduct.Find(idProduct));
        }
    }
}
