using AppTienda.Core.Domain.Common;
using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public ExpenseCategory Category { get; set; } = ExpenseCategory.Otros;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
