using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductSupplier.Models.Interface;
using ProductSupplier.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<CategoryOutputModel>> GetAllAsync()
        {
            var categories = await _repositoryCategory.GetAllAsync();
            return MappingManyToManyModel.CategotyListOutput(categories);
        }

        [HttpGet("{id}", Name = "Category")]
        public async Task<CategoryOutputModel> GetUnitAsync(string id)
        {
            var Id = Guid.Parse(id);
            var item = await _repositoryCategory.FindAsync(Id);
            return MappingManyToManyModel.CategoryOutput(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
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
            await _repositoryCategory.AddAsync(category);
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(category));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var Id = Guid.Parse(id);
            var item = await _repositoryCategory.FindAsync(Id);
            if (item == null)
            {
                return NotFound();
            }
            await _repositoryCategory.DeleteAsync(item);
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(item));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] CategoryOutputModel itemInput)
        {
            var Id = Guid.Parse(id);
            if (id == null || itemInput.Id != Id)
            {
                return BadRequest();
            }

            var dbItem = MappingManyToManyModel.CategoryInput(itemInput);
            foreach (var product in dbItem.Products)
            {
                if (_repositoryProduct.FindAsync(product.Id) == null)
                {
                    return BadRequest();
                }
            }
            await _repositoryCategory.UpdateAsync(id, dbItem);
            var dbItemUpdate = await _repositoryCategory.FindAsync(id);
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(dbItemUpdate));
        }

        [HttpPut("{idCategory}/{idProduct}")]
        public async Task<IActionResult> AddProductToCategoryAsync(string idCategory, string idProduct)
        {
            var category = await _repositoryCategory.FindAsync(idCategory);
            if (category == null)
            {
                return NotFound();
            }
            var product = await _repositoryProduct.FindAsync(idProduct);
            if (category.Products != null)
            {
                if (category.Products.FirstOrDefault(c => c.Id == product.Id) == null)
                {
                    category.Products.Add(product);
                    await _repositoryCategory.UpdateAsync(idCategory, category);
                }
            }
            else
            {
                category.Products = new List<Product>()
                {
                    product
                };
                await _repositoryCategory.UpdateAsync(idCategory, category);
            }
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(await _repositoryCategory.FindAsync(idCategory)));
        }

        [HttpDelete("{idCategory}/{idProduct}")]
        public async Task<IActionResult> DeleteProductFromCategoryAsync(string idCategory, string idProduct)
        {
            var category = await _repositoryCategory.FindAsync(idCategory);
            if (category == null)
            {
                return NotFound();
            }
            var product = await _repositoryProduct.FindAsync(idProduct);
            if (category.Products != null)
            {
                if (category.Products.FirstOrDefault(c => c.Id == product.Id) == null)
                {
                    category.Products.Remove(product);
                    await _repositoryCategory.UpdateAsync(idCategory, category);
                }
            }
            return new ObjectResult(MappingManyToManyModel.CategoryOutput(await _repositoryCategory.FindAsync(idCategory)));
        }
    }
}
