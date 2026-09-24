using AppTienda.Core.Application.Dtos.Finance;
using AppTienda.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppTienda.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinanceService _financeService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(
            IFinanceService financeService,
            IProductService productService,
            ICategoryService categoryService)
        {
            _financeService = financeService;
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(DateFilterType filterType = DateFilterType.EsteMes)
        {
            var filter = new FinanceFilterDto { FilterType = filterType };
            var summary = await _financeService.GetFinanceSummaryAsync(filter);
            ViewBag.LowStockProducts = await _productService.GetLowStockProductsAsync(3);
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.AvailableProducts = await _productService.GetAvailableProductsAsync();
            return View(summary);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
