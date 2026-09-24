using System.ComponentModel.DataAnnotations;
using AppTienda.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace AppTienda.Core.Application.Dtos.Product
{
    public class SaveProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
        public string? Description { get; set; }

        public string? Sku { get; set; }

        [Required(ErrorMessage = "El precio de compra/costo es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "El precio de venta es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
        [Range(0, 999999, ErrorMessage = "El stock debe ser 0 o superior.")]
        public int Stock { get; set; } = 1;

        public ProductStatus Status { get; set; } = ProductStatus.Disponible;

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
