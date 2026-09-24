using AppTienda.Core.Application.Dtos.Finance;

namespace AppTienda.Core.Application.Interfaces
{
    public interface IFinanceService
    {
        Task<FinanceSummaryDto> GetFinanceSummaryAsync(FinanceFilterDto filter);
    }
}
