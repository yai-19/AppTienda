using AppTienda.Core.Application.Dtos.Product;

namespace AppTienda.Core.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(int? categoryId = null, string? searchTerm = null);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<SaveProductDto?> GetSaveByIdAsync(int id);
        Task<SaveProductDto> AddAsync(SaveProductDto dto, string? imageUrl = null);
        Task UpdateAsync(SaveProductDto dto, string? imageUrl = null);
        Task DeleteAsync(int id);
        Task<List<ProductDto>> GetAvailableProductsAsync();
        Task<List<ProductDto>> GetLowStockProductsAsync(int threshold = 3);
        Task UpdateStockAndStatusAsync(int productId, int quantitySold);
    }
}
