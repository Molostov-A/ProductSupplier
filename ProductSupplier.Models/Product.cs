using System;
using System.Collections.Generic;

namespace ProductSupplier.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<Category> Categories { get; set; }
    }
}
