using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Application.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public ProductStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Expected profit per unit
        public decimal EstimatedUnitProfit => SalePrice - PurchasePrice;
        public decimal EstimatedMarginPercentage => PurchasePrice > 0 ? ((SalePrice - PurchasePrice) / PurchasePrice) * 100 : 100;
    }
}
