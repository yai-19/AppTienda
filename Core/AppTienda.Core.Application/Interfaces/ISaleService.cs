using AppTienda.Core.Application.Dtos.Sale;

namespace AppTienda.Core.Application.Interfaces
{
    public interface ISaleService
    {
        Task<List<SaleDto>> GetAllAsync();
        Task<SaleDto?> GetByIdAsync(int id);
        Task<SaleDto> RegisterSaleAsync(SaveSaleDto dto);
        Task<List<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task DeleteAsync(int id);
    }
}
