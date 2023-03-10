using System.Collections.Generic;
using ProductSupplier.Models;
using ProductSupplier.WebApi.OutputModels;

namespace ProductSupplier.WebApi.Mapper
{
    /// <summary>
    /// Статический класс для решения проблемы циклических ссылок,
    /// для таблиц, связанных по принципу "многое-ко-многим"
    /// </summary>
    public static class MappingManyToManyModel
    {
        /// <summary>
        /// Конвертировать List<Product> в список для вывода в JSON List<ProductOutputModel> без циклических ссылок
        /// </summary>
        /// <param name="listProducts"></param>
        /// <returns></returns>
        public static List<ProductOutputModel> ProductsListOutput(List<Product> listProducts)
        {
            var listProductsOutputModel = new List<ProductOutputModel>();
            foreach (var product in listProducts)
            {
                listProductsOutputModel.Add(ProductOutput(product));
            }
            return listProductsOutputModel;
        }

        /// <summary>
        /// Перевод модели БД Product в модель вывода данных ProductOutputModel, без циклических ссылок
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
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
                    Price = inputModel.Price,
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Categories = new List<CategoryInProductOutputModel>()
                };
            }
            return outputModel;
        }

        /// <summary>
        /// Конвектрировать модель для вывода данных ProductOutputModel в модель для БД - Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    Categories = new List<Category>()
                };
            }
            return inputModel;
        }

        /// <summary>
        /// Конвертировать List<Category> в список для вывода в JSON List<CategoryOutputModel> без циклических ссылок
        /// </summary>
        /// <param name="listProducts"></param>
        /// <returns></returns>
        public static List<CategoryOutputModel> CategoryListOutput(List<Category> listCategory)
        {
            var listCategoryOutputModel = new List<CategoryOutputModel>();
            foreach (var category in listCategory)
            {
                listCategoryOutputModel.Add(CategoryOutput(category));
            }
            return listCategoryOutputModel;
        }

        /// <summary>
        /// Перевод модели БД Category в модель вывода данных CategoryOutputModel, без циклических ссылок
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
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
                    Products = new List<ProductInCategoryOutputModel>()
                };
            }
            return outputModel;
        }

        /// <summary>
        /// Конвектрировать модель для вывода данных CategoryOutputModel в модель для БД - Category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    Products = new List<Product>()
                };
            }
            return inputModel;
        }
    }
}