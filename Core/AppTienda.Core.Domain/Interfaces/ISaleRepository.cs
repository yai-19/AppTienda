using AppTienda.Core.Domain.Entities;

namespace AppTienda.Core.Domain.Interfaces
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        Task<List<Sale>> GetSalesWithProductAsync();
        Task<List<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
