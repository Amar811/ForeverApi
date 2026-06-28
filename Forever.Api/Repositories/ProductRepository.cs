using Forever.Api.DTOs.Product;
using Forever.Api.Models;
using AuthDemo.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Forever.Api.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Adds a new product to the database and returns the created product with the generated ProductId
        /// </summary>
        public async Task<Product> AddProductAsync(Product product)
        {
            await _applicationDbContext.Products.AddAsync(product);
            await _applicationDbContext.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Gets all active products with their category information using eager loading
        /// </summary>
        public async Task<List<ProductResponseDto>> GetAllProductAsync()
        {
            return await _applicationDbContext.Products
                         .Where(p => p.IsActive)
                         .Include(p => p.Category)
                         .Select(p => new ProductResponseDto
                         {
                             ProductId = p.ProductId,
                             ProductName = p.ProductName,
                             Description = p.Description,
                             Price = p.Price,
                             Quantity = p.Quantity,
                             ImageUrl = p.ImageUrl,
                             CategoryName = p.Category.CategoryName
                         })
                         .ToListAsync();
        }
    }
}
