using AppTienda.Core.Domain.Entities;

namespace AppTienda.Core.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetCategoriesWithProductsAsync();
    }
}
