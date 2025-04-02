using BusinessLayer.DTO.Category;
using BusinessLayer.DTO.Product;
using BusinessLayer.Manager.IManager;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Manager
{
    public class ProductManager:IProductManager
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductManager(IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task AddProduct(CreateProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                ImagePath = productDTO.ImagePath,
                CategoryId = productDTO.CategoryId
            };

            await _productRepository.CreateProductAsync(product);

        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync(); 

            var categoriesDTO = new List<CategoryDTO>();  

            foreach (var category in categories)
            {
                categoriesDTO.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return categoriesDTO;
        }

        public async Task<List<ProductCardDTO>> GetAllProductByCategoryName(string CategoryName)
        {
            var productsFromRepo = await _productRepository.GetProductByCategoryNameAsync(CategoryName);

            var products = new List<ProductCardDTO>();

            foreach (var product in productsFromRepo)
            {
                var productDTO = new ProductCardDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImagePath = product.ImagePath
                };

                products.Add(productDTO);
            }

            return products;
        }

        public async Task<ProductCardDTO> GetProductByIdAsync(int Id)
        {
            var product = await _productRepository.GetProductByIdAsync(Id);

            var productDTO = new ProductCardDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImagePath = product.ImagePath
            };
            return productDTO;
        }

        public async Task Delete(int Id)
        {
          await  _productRepository.DeleteProductAsync(Id);
        }
    }
}
