using AppTienda.Core.Application.Dtos.Sale;
using AppTienda.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppTienda.App.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;

        public SaleController(ISaleService saleService, IProductService productService)
        {
            _saleService = saleService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _saleService.GetAllAsync();
            return View(sales);
        }

        public async Task<IActionResult> Create(int? productId)
        {
            var availableProducts = await _productService.GetAvailableProductsAsync();
            if (!availableProducts.Any())
            {
                TempData["Warning"] = "No hay productos con stock disponible para vender. Agrega o actualiza el stock primero.";
                return RedirectToAction("Index", "Product");
            }

            ViewBag.Products = new SelectList(availableProducts, "Id", "Name", productId);
            ViewBag.ProductsList = availableProducts;

            var model = new SaveSaleDto
            {
                SaleDate = DateTime.Now,
                Quantity = 1
            };

            if (productId.HasValue)
            {
                var selected = availableProducts.FirstOrDefault(p => p.Id == productId.Value);
                if (selected != null)
                {
                    model.ProductId = selected.Id;
                    model.UnitPrice = selected.SalePrice;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveSaleDto dto)
        {
            if (!ModelState.IsValid)
            {
                var availableProducts = await _productService.GetAvailableProductsAsync();
                ViewBag.Products = new SelectList(availableProducts, "Id", "Name", dto.ProductId);
                ViewBag.ProductsList = availableProducts;
                return View(dto);
            }

            try
            {
                var sale = await _saleService.RegisterSaleAsync(dto);
                TempData["Success"] = $"¡Venta registrada con éxito! Ganancia generada: ${sale.Profit:N2}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var availableProducts = await _productService.GetAvailableProductsAsync();
                ViewBag.Products = new SelectList(availableProducts, "Id", "Name", dto.ProductId);
                ViewBag.ProductsList = availableProducts;
                return View(dto);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _saleService.DeleteAsync(id);
            TempData["Success"] = "Venta anulada y stock devuelto al inventario.";
            return RedirectToAction(nameof(Index));
        }
    }
}
