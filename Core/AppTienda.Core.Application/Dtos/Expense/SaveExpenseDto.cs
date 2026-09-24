using System.ComponentModel.DataAnnotations;
using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Application.Dtos.Expense
{
    public class SaveExpenseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha del gasto es obligatoria.")]
        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Seleccione la categoría de gasto.")]
        public ExpenseCategory Category { get; set; } = ExpenseCategory.Otros;

        [Required(ErrorMessage = "La descripción del gasto es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede superar 200 caracteres.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto del gasto es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Amount { get; set; }

        public string? Notes { get; set; }
    }
}
