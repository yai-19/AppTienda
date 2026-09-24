using AppTienda.Core.Domain.Entities;

namespace AppTienda.Core.Domain.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<List<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
