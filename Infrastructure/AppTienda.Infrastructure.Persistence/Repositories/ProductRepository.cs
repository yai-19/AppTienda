using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Interfaces;
using AppTienda.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AppTienda.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppTiendaDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithCategoryByIdAsync(int id)
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetLowStockProductsAsync(int threshold = 3)
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.Stock <= threshold)
                .OrderBy(p => p.Stock)
                .ToListAsync();
        }
    }
}
