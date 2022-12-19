using System;

namespace ProductSupplier.WebApi.OutputModels
{
    public class ProductInCategoryOutputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}