using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Interfaces;
using AppTienda.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AppTienda.Infrastructure.Persistence.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(AppTiendaDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }
    }
}
