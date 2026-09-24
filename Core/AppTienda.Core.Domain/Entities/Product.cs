using AppTienda.Core.Domain.Common;
using AppTienda.Core.Domain.Enums;

namespace AppTienda.Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Disponible;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
