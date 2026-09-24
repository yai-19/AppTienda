using AppTienda.Core.Domain.Common;
using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Profit { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;
        public string? CustomerName { get; set; }
        public string? Notes { get; set; }
    }
}
