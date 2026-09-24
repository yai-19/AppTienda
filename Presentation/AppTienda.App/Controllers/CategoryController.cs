using AppTienda.Core.Application.Dtos.Category;
using AppTienda.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppTienda.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveCategoryDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", dto);
            }

            await _categoryService.AddAsync(dto);
            TempData["Success"] = "Categoría creada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetSaveByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View("Save", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", dto);
            }

            await _categoryService.UpdateAsync(dto);
            TempData["Success"] = "Categoría actualizada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category != null && category.ProductsCount > 0)
            {
                TempData["Error"] = "No puedes eliminar una categoría que tiene productos asociados.";
                return RedirectToAction(nameof(Index));
            }

            await _categoryService.DeleteAsync(id);
            TempData["Success"] = "Categoría eliminada con éxito.";
            return RedirectToAction(nameof(Index));
        }
    }
}
