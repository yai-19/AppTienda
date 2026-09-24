using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Interfaces;
using AppTienda.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AppTienda.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppTiendaDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Category>> GetCategoriesWithProductsAsync()
        {
            return await _dbContext.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }
    }
}
