using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Application.Dtos.Sale
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImageUrl { get; set; }
        public string? CategoryName { get; set; }

        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Profit { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public string? CustomerName { get; set; }
        public string? Notes { get; set; }

        public decimal ProfitMarginPercentage => TotalCost > 0 ? (Profit / TotalCost) * 100 : 100;
    }
}
