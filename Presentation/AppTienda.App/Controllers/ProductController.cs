using AppTienda.Core.Application.Dtos.Product;
using AppTienda.Core.Application.Interfaces;
using AppTienda.Infrastructure.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppTienda.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUploadFileService _uploadFileService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IUploadFileService uploadFileService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _uploadFileService = uploadFileService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? searchTerm)
        {
            var products = await _productService.GetAllAsync(categoryId, searchTerm);
            var categories = await _categoryService.GetAllAsync();
            
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.CurrentCategory = categoryId;
            ViewBag.SearchTerm = searchTerm;

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            if (!categories.Any())
            {
                TempData["Warning"] = "Debes crear al menos una categoría antes de registrar un producto.";
                return RedirectToAction("Create", "Category");
            }

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View("Save", new SaveProductDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View("Save", dto);
            }

            string? imageUrl = null;
            if (dto.ImageFile != null)
            {
                imageUrl = await _uploadFileService.UploadFileAsync(dto.ImageFile, "products");
            }

            await _productService.AddAsync(dto, imageUrl);
            TempData["Success"] = "Producto registrado con éxito.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetSaveByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View("Save", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View("Save", dto);
            }

            string? imageUrl = dto.ImageUrl;
            if (dto.ImageFile != null)
            {
                imageUrl = await _uploadFileService.UploadFileAsync(dto.ImageFile, "products", dto.ImageUrl);
            }

            await _productService.UpdateAsync(dto, imageUrl);
            TempData["Success"] = "Producto actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product != null && !string.IsNullOrEmpty(product.ImageUrl))
            {
                _uploadFileService.DeleteFile(product.ImageUrl);
            }

            await _productService.DeleteAsync(id);
            TempData["Success"] = "Producto eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
