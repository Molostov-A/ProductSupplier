using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductSupplier.Models;
using ProductSupplier.Models.Interface;
using ProductSupplier.WebApi.Mapper;
using ProductSupplier.WebApi.OutputModels;
using ProductSupplier.WebApi.ViewModels;

namespace ProductSupplier.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        
        private readonly IRepository<Product> _repositoryProduct;
        private readonly IRepository<Category> _repositoryCategory;
        private readonly ILogger<ProductController> _logger;


        public ProductController(ILogger<ProductController> logger, IRepository<Product> repositoryProduct, IRepository<Category> repositoryCategory)
        {
            _logger = logger;
            _repositoryProduct = repositoryProduct;
            _repositoryCategory = repositoryCategory;
        }

        
        public async Task<List<ProductOutputModel>> GetAllAsync()
        {
            return MappingManyToManyModel.ProductsListOutput(await _repositoryProduct.GetAllAsync());
        }

        [HttpGet("{id}", Name = "Product")]
        public async Task<IActionResult> GetUnitAsync(string id)
        {
            var productId = Guid.Parse(id);
            var product = await _repositoryProduct.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(MappingManyToManyModel.ProductOutput(product));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductViewModel productViewModel)
        {
            if (productViewModel == null)
            {
                return NotFound();
            }
            var categories = new List<Category>();
            foreach (var idGuid in productViewModel.Categories)
            {
                var category = await _repositoryCategory.FindAsync(idGuid);
                
                if (category == null)
                {
                    return BadRequest();
                }
                categories.Add(category);
            }
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                Categories = categories
            };
            await _repositoryProduct.AddAsync(product);
            return Ok(MappingManyToManyModel.ProductOutput(product));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] ProductOutputModel itemInput)
        {
            var Id = Guid.Parse(id);
            if (id == null || itemInput.Id != Id)
            {
                return BadRequest();
            }

            var dbItem = MappingManyToManyModel.ProductInput(itemInput);
            foreach (var category in dbItem.Categories)
            {
                if (await _repositoryCategory.FindAsync(category.Id) == null)
                {
                    return BadRequest();
                }
            }
            await _repositoryProduct.UpdateAsync(id, dbItem);
            var dbItemUpdate = await _repositoryProduct.FindAsync(id);
            return Ok(MappingManyToManyModel.ProductOutput(dbItemUpdate));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsinc(string id)
        {
            var Id = Guid.Parse(id);
            var item = await _repositoryProduct.FindAsync(Id);
            if (item == null)
            {
                return NotFound();
            }
            await _repositoryProduct.DeleteAsync(item);
            return Ok(MappingManyToManyModel.ProductOutput(item));
        }

        [HttpPut("{idProduct}/{idCategory}")]
        public async Task<IActionResult> AddCategoryToProductAsync(string idProduct, string idCategory)
        {
            var product = await _repositoryProduct.FindAsync(idProduct);
            if (product == null)
            {
                return NotFound();
            }
            var category =  await _repositoryCategory.FindAsync(idCategory);
            if (product.Categories != null)
            {
                if (product.Categories.FirstOrDefault(c => c.Id == category.Id) == null)
                {
                    product.Categories.Add(category);
                    await _repositoryProduct.UpdateAsync(idProduct, product);
                }
            }
            else
            {
                product.Categories = new List<Category>()
                {
                    category
                };
                await _repositoryProduct.UpdateAsync(idProduct, product);
            }

            return Ok(MappingManyToManyModel.ProductOutput(product));
        }

        [HttpDelete("{idProduct}/{idCategory}")]
        public async Task<IActionResult> RemoveCategoryFromProductAsync(string idProduct, string idCategory)
        {
            var product = await _repositoryProduct.FindAsync(idProduct);
            if (product == null)
            {
                return NotFound();
            }
            var category = await _repositoryCategory.FindAsync(idCategory);
            if (product.Categories != null)
            {
                if (product.Categories.FirstOrDefault(c => c.Id == category.Id) != null)
                {
                    product.Categories.Remove(category);
                    await _repositoryProduct.UpdateAsync(idProduct, product);
                }
            }
            var lastProduct = await _repositoryProduct.FindAsync(idProduct);
            return Ok(MappingManyToManyModel.ProductOutput(lastProduct));
        }
    }
}
