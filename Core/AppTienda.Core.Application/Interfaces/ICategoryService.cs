using AppTienda.Core.Application.Dtos.Category;

namespace AppTienda.Core.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<SaveCategoryDto?> GetSaveByIdAsync(int id);
        Task<SaveCategoryDto> AddAsync(SaveCategoryDto dto);
        Task UpdateAsync(SaveCategoryDto dto);
        Task DeleteAsync(int id);
    }
}
