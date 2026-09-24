using System.ComponentModel.DataAnnotations;

namespace AppTienda.Core.Application.Dtos.Category
{
    public class SaveCategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
        public string? Description { get; set; }
    }
}
