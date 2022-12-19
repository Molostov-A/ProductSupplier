using System.Collections.Generic;
using System;

namespace ProductSupplier.WebApi.OutputModels
{
    public class CategoryOutputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductInCategoryOutputModel> Products { get; set; }
    }
}