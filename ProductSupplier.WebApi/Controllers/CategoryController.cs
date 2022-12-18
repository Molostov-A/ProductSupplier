using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductSupplier.Models.Interface;
using ProductSupplier.Models;
using System.Collections.Generic;
using System;
using ProductSupplier.WebApi.ViewModels;

namespace ProductSupplier.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _repository;
        private readonly ILogger<CategoryController> _logger;


        public CategoryController(ILogger<CategoryController> logger, IRepository<Category> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        //[ScriptIgnore]
        public IEnumerable<Category> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id}", Name = "Category")]
        public Category GetUnit(string id)
        {
            var Id = Guid.Parse(id);
            var item = _repository.Find(Id);
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
            _repository.Add(category);
            return new ObjectResult(category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var Id = Guid.Parse(id);
            var value = _repository.Find(Id);
            if (value == null)
            {
                return NotFound();
            }
            _repository.Delete(value);
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
            var dbItem = _repository.Find(id);
            if (dbItem == null)
            {
                return NotFound();
            }
            _repository.Update(id, dbItem);
            var dbItemUpdate = _repository.Find(id);
            return new ObjectResult(dbItemUpdate);
        }
    }
}
