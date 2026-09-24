using AppTienda.Core.Domain.Entities;

namespace AppTienda.Core.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsWithCategoryAsync();
        Task<Product?> GetProductWithCategoryByIdAsync(int id);
        Task<List<Product>> GetLowStockProductsAsync(int threshold = 3);
    }
}
