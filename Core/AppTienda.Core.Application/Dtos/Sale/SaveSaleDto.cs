using System.ComponentModel.DataAnnotations;
using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Application.Dtos.Sale
{
    public class SaveSaleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria.")]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar un producto para vender.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un producto válido.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, 9999, ErrorMessage = "La cantidad mínima a vender es 1.")]
        public int Quantity { get; set; } = 1;

        [Required(ErrorMessage = "El precio de venta es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
        public decimal UnitPrice { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;

        public string? CustomerName { get; set; }
        public string? Notes { get; set; }
    }
}
