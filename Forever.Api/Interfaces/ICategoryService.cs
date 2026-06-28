using Forever.Api.DTOs.Category;

namespace Forever.Api.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> AddCategory(AddCategoryRequestDto request);

        Task<List<CategoryResponseDto>> GetAllCategory();

        Task<CategoryResponseDto?> GetCategoryById(int id);

        Task<CategoryResponseDto?> UpdateCategory(int id, UpdateCategoryRequestDto request);

        Task<bool> DeleteCategory(int id);
    }
}
