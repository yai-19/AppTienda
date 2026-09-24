using AppTienda.Core.Application.Dtos.Expense;

namespace AppTienda.Core.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseDto>> GetAllAsync();
        Task<ExpenseDto?> GetByIdAsync(int id);
        Task<SaveExpenseDto?> GetSaveByIdAsync(int id);
        Task<SaveExpenseDto> AddAsync(SaveExpenseDto dto);
        Task UpdateAsync(SaveExpenseDto dto);
        Task DeleteAsync(int id);
        Task<List<ExpenseDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
