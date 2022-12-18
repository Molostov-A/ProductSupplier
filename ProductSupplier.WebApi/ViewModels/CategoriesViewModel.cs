using System.Collections.Generic;
using System;

namespace ProductSupplier.WebApi.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> Products { get; set; }
    }
}