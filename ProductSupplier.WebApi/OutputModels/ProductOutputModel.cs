using ProductSupplier.Models;
using System.Collections.Generic;
using System;

namespace ProductSupplier.WebApi.OutputModels
{
    public class ProductOutputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public List<CategoryInProductOutputModel> Categories { get; set; }
    }
}