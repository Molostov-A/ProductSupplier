using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductSupplier.Models.Interface;
using ProductSupplier.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ProductSupplier.WebApi.Mapper;
using ProductSupplier.WebApi.OutputModels;
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
        public IEnumerable<CategoryOutputModel> GetAll()
        {
            return MappingManyToManyModel.CategotyListOutput(_repositoryCategory.GetAll());
        }

        [HttpGet("{id}", Name = "Category")]
        public CategoryOutputModel GetUnit(string id)
        {
            var Id = Guid.Parse(id);
            var item = _repositoryCategory.Find(Id);
            return MappingManyToManyModel.CategoryOutput(item);
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
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(category));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var Id = Guid.Parse(id);
            var item = _repositoryCategory.Find(Id);
            if (item == null)
            {
                return NotFound();
            }
            _repositoryCategory.Delete(item);
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(item));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(string id, [FromBody] CategoryOutputModel itemInput)
        {
            var Id = Guid.Parse(id);
            if (id == null || itemInput.Id != Id)
            {
                return BadRequest();
            }

            var dbItem = MappingManyToManyModel.CategoryInput(itemInput);
            foreach (var product in dbItem.Products)
            {
                if (_repositoryProduct.Find(product.Id) == null)
                {
                    return BadRequest();
                }
            }
            _repositoryCategory.Update(id, dbItem);
            var dbItemUpdate = _repositoryCategory.Find(id);
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(dbItemUpdate));
        }

        [HttpPut("{idCategory}/{idProduct}")]
        public IActionResult AddProductToCategory(string idCategory, string idProduct)
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
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(_repositoryCategory.Find(idCategory)));
        }

        [HttpDelete("{idCategory}/{idProduct}")]
        public IActionResult DeleteProductFromCategory(string idCategory, string idProduct)
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
                    category.Products.Remove(product);
                    _repositoryCategory.Update(idCategory, category);
                }
            }
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(_repositoryCategory.Find(idCategory)));
        }
    }
}
