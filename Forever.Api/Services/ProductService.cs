using Forever.Api.DTOs.Product;
using Forever.Api.Interfaces;
using Forever.Api.Models;
using Forever.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Forever.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDto> AddProduct(AddProductRequestDto request, int userId)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                CreatedBy = userId,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // Save product to database and get the generated ProductId
            var createdProduct = await _productRepository.AddProductAsync(product);

            return new ProductResponseDto
            {
                ProductId = createdProduct.ProductId,
                ProductName = createdProduct.ProductName,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                Quantity = createdProduct.Quantity,
                ImageUrl = createdProduct.ImageUrl,
                //CategoryName = createdProduct.Category?.CategoryName ?? string.Empty
            };
        }

        public async Task<List<ProductResponseDto>> GetAllProducts()
        {
            return await _productRepository.GetAllProductAsync();
        }
    }
}
