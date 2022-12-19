using System;

namespace ProductSupplier.WebApi.OutputModels
{
    public class CategoryInProductOutputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}