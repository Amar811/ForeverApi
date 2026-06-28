using Forever.Api.Models; // Add this line if Category is defined in Forever.Api.Models

namespace Forever.Api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category);

        Task<List<Category>> GetAllCategoryAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task<Category?> UpdateCategoryAsync(Category category);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
