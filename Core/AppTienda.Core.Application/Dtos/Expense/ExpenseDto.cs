using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Application.Dtos.Expense
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
