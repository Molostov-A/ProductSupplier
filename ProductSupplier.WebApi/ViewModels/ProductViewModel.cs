using System;
using System.Collections.Generic;

namespace ProductSupplier.WebApi.ViewModels
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        //public List<Guid> Categories { get; set; }

    }
}