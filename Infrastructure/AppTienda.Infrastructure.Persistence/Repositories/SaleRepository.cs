using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Interfaces;
using AppTienda.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AppTienda.Infrastructure.Persistence.Repositories
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        public SaleRepository(AppTiendaDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Sale>> GetSalesWithProductAsync()
        {
            return await _dbContext.Sales
                .Include(s => s.Product)
                    .ThenInclude(p => p != null ? p.Category : null)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<List<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Sales
                .Include(s => s.Product)
                    .ThenInclude(p => p != null ? p.Category : null)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }
    }
}
