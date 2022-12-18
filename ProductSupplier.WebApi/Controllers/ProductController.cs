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

        
        public IEnumerable<ProductOutputModel> GetAll()
        {
            return MappingManyToManyModel.ProductsListOutput(_repositoryProduct.GetAll());
        }

        [HttpGet("{id}", Name = "Product")]
        public ProductOutputModel GetUnit(string id)
        {
            var productId = Guid.Parse(id);
            var product = _repositoryProduct.Find(productId);
            return MappingManyToManyModel.ProductOutput(product);
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (productViewModel == null)
            {
                return NotFound();
            }
            var categories = new List<Category>();
            foreach (var idGuid in productViewModel.Categories)
            {
                var category = _repositoryCategory.Find(idGuid);
                
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
            _repositoryProduct.Add(product);
            return new ObjectResult(MappingManyToManyModel.ProductOutput(product));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(string id, [FromBody] ProductOutputModel itemInput)
        {
            var Id = Guid.Parse(id);
            if (id == null || itemInput.Id != Id)
            {
                return BadRequest();
            }

            var dbItem = MappingManyToManyModel.ProductInput(itemInput);
            foreach (var category in dbItem.Categories)
            {
                if (_repositoryCategory.Find(category.Id) == null)
                {
                    return BadRequest();
                }
            }
            _repositoryProduct.Update(id, dbItem);
            var dbItemUpdate = _repositoryProduct.Find(id);
            return new ObjectResult(MappingManyToManyModel.ProductOutput(dbItemUpdate));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var Id = Guid.Parse(id);
            var item = _repositoryProduct.Find(Id);
            if (item == null)
            {
                return NotFound();
            }
            _repositoryProduct.Delete(item);
            return new ObjectResult(MappingManyToManyModel.ProductOutput(item));
        }

        [HttpPut("{idProduct}/{idCategory}")]
        public IActionResult AddCategoryToProduct(string idProduct, string idCategory)
        {
            var product = _repositoryProduct.Find(idProduct);
            if (product == null)
            {
                return NotFound();
            }
            var category = _repositoryCategory.Find(idCategory);
            if (product.Categories != null)
            {
                if (product.Categories.FirstOrDefault(c => c.Id == category.Id) == null)
                {
                    product.Categories.Add(category);
                    _repositoryProduct.Update(idProduct, product);
                }
            }
            else
            {
                product.Categories = new List<Category>()
                {
                    category
                };
                _repositoryProduct.Update(idProduct, product);
            }

            return new ObjectResult(MappingManyToManyModel.ProductOutput(product));
        }

        [HttpDelete("{idProduct}/{idCategory}")]
        public IActionResult RemoveCategoryFromProduct(string idProduct, string idCategory)
        {
            var product = _repositoryProduct.Find(idProduct);
            if (product == null)
            {
                return NotFound();
            }
            var category = _repositoryCategory.Find(idCategory);
            if (product.Categories != null)
            {
                if (product.Categories.FirstOrDefault(c => c.Id == category.Id) == null)
                {
                    product.Categories.Remove(category);
                    _repositoryProduct.Update(idProduct, product);
                }
            }

            return new ObjectResult(MappingManyToManyModel.ProductOutput(product));
        }
    }
}
