using AuthDemo.Api.Data;
using Forever.Api.DTOs.Product;
using Forever.Api.Interfaces.Product;
using Forever.Api.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Forever.Api.Services.Product
{
    public class ProductService : IProductService
    {
        private  readonly ApplicationDbContext _applicationDbContext;

        public ProductService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ProductResponseDto> AddProduct(AddProductRequestDto request, int userId)
        {
            var product = new Models.Product.Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl,
                CreatedBy = userId,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _applicationDbContext.Add(product);
            await _applicationDbContext.SaveChangesAsync();

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl

            };
        }


        public async Task<List<ProductResponseDto>> GetAllProducts()
        {
            return await _applicationDbContext.Products
             .Where(x => x.IsActive)
             .Select(x => new ProductResponseDto
             {
                 ProductId = x.ProductId,
                 ProductName = x.ProductName,
                 Description = x.Description,
                 Price = x.Price,
                 Quantity = x.Quantity,
                 ImageUrl = x.ImageUrl
             })
             .ToListAsync();
        }
    }
}
