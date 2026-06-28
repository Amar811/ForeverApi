using Forever.Api.DTOs.Category;
using Forever.Api.Interfaces;
using Forever.Api.Models;

namespace Forever.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDto> AddCategory(AddCategoryRequestDto request)
        {
            var category = new Category
            {
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription,
                CategoryDate = DateTime.Now
            };

            var result = await _categoryRepository.AddCategoryAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = result.CategoryId,
                CategoryName = result.CategoryName,
                CategoryDescription = result.CategoryDescription
            };
        }

        public async Task<List<CategoryResponseDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllCategoryAsync();

            return categories.Select(x => new CategoryResponseDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                CategoryDescription = x.CategoryDescription
            }).ToList();
        }

        public async Task<CategoryResponseDto?> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription
            };
        }

        public async Task<CategoryResponseDto?> UpdateCategory(int id, UpdateCategoryRequestDto request)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
                return null;

            category.CategoryName = request.CategoryName;
            category.CategoryDescription = request.CategoryDescription;

            var result = await _categoryRepository.UpdateCategoryAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = result!.CategoryId,
                CategoryName = result.CategoryName,
                CategoryDescription = result.CategoryDescription
            };
        }

        public async Task<bool> DeleteCategory(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}