using System.Collections.Generic;
using ProductSupplier.Models;
using ProductSupplier.WebApi.OutputModels;

namespace ProductSupplier.WebApi.Mapper
{
    public static class MappingManyToManyModel
    {
        public static List<ProductOutputModel> ProductsListOutput(List<Product> listProducts)
        {
            var listProductsOutputModel = new List<ProductOutputModel>();
            foreach (var product in listProducts)
            {
                listProductsOutputModel.Add(ProductOutput(product));
            }
            return listProductsOutputModel;
        }
        public static ProductOutputModel ProductOutput(Product inputModel)
        {
            var outputModel = new ProductOutputModel();
            if (inputModel.Categories != null)
            {
                var categories = new List<CategoryInProductOutputModel>();
                foreach (var category in inputModel.Categories)
                {
                    categories.Add(new CategoryInProductOutputModel()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description
                    });
                }
                outputModel = new ProductOutputModel()
                {
                    Id = inputModel.Id,
                    Price = inputModel.Price,
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Categories = categories
                };
            }
            else
            {
                outputModel = new ProductOutputModel()
                {
                    Id = inputModel.Id,
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Categories = null
                };
            }
            return outputModel;
        }
        public static Product ProductInput(ProductOutputModel model)
        {
            var inputModel = new Product();
            if (model.Categories != null)
            {
                var categories = new List<Category>();
                foreach (var category in model.Categories)
                {
                    categories.Add(new Category()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description
                    });
                }
                inputModel = new Product()
                {
                    Id = model.Id,
                    Price = model.Price,
                    Name = model.Name,
                    Description = model.Description,
                    Categories = categories
                };
            }
            else
            {
                inputModel = new Product()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Categories = null
                };
            }
            return inputModel;
        }
        public static List<CategoryOutputModel> CategotyListOutput(List<Category> listCategory)
        {
            var listCategoryOutputModel = new List<CategoryOutputModel>();
            foreach (var category in listCategory)
            {
                listCategoryOutputModel.Add(CategoryOutput(category));
            }
            return listCategoryOutputModel;
        }
        public static CategoryOutputModel CategoryOutput(Category inputModel)
        {
            var outputModel = new CategoryOutputModel();
            if (inputModel.Products != null)
            {
                var products = new List<ProductInCategoryOutputModel>();
                foreach (var product in inputModel.Products)
                {
                    products.Add(new ProductInCategoryOutputModel()
                    {
                        Id = product.Id,
                        Price = product.Price,
                        Name = product.Name,
                        Description = product.Description
                    });
                }
                outputModel = new CategoryOutputModel()
                {
                    Id = inputModel.Id,
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Products = products
                };
            }
            else
            {
                outputModel = new CategoryOutputModel()
                {
                    Id = inputModel.Id,
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Products = null
                };
            }
            return outputModel;
        }

        public static Category CategoryInput(CategoryOutputModel model)
        {
            var inputModel = new Category();

            if (model.Products != null)
            {
                var products = new List<Product>();
                foreach (var product in model.Products)
                {
                    products.Add(new Product()
                    {
                        Id = product.Id,
                        Price = product.Price,
                        Name = product.Name,
                        Description = product.Description
                    });
                }
                inputModel = new Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Products = products
                };
            }
            else
            {
                inputModel = new Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Products = null
                };
            }
            return inputModel;
        }
    }
}