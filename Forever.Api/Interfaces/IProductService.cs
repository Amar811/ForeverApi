using Forever.Api.DTOs.Product;

namespace Forever.Api.Interfaces
{
    public interface  IProductService
    {
        Task<ProductResponseDto> AddProduct(AddProductRequestDto request, int userId);

        Task<List<ProductResponseDto>> GetAllProducts();
    }
}
