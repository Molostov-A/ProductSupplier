using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductSupplier.Models;
using ProductSupplier.Models.Interface;
using ProductSupplier.WebApi.ViewModels;

namespace ProductSupplier.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger<ProductController> _logger;


        public ProductController(ILogger<ProductController> logger, IRepository<Product> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        
        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id}", Name = "Product")]
        public Product GetUnit(string id)
        {
            var productId = Guid.Parse(id);
            var product = _repository.Find(productId);
            return product;
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (productViewModel == null)
            {
                return BadRequest();
            }

            var categories = new List<Category>();
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                Categories = categories
            };
            _repository.Add(product);
            return new ObjectResult(product);
        }

        [HttpDelete("{idProduct}")]
        public IActionResult Delete(string idProduct)
        {
            var productId = Guid.Parse(idProduct);
            var product = _repository.Find(productId);
            if (product == null)
            {
                return BadRequest();
            }
            _repository.Delete(product);
            return new ObjectResult(product);
        }
    }
}
