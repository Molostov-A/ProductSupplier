using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductSupplier.Models;
using ProductSupplier.Models.Interface;

namespace ProductSupplier.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;


        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        //GET/dot
        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel productViewModel)
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
            _productRepository.Add(product);
            return new ObjectResult(product);
        }
    }

    public class CategoriesViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
