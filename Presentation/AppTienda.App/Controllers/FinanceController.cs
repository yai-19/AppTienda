using AppTienda.Core.Application.Dtos.Finance;
using AppTienda.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppTienda.App.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IActionResult> Index(DateFilterType filterType = DateFilterType.EsteMes, DateTime? startDate = null, DateTime? endDate = null)
        {
            var filter = new FinanceFilterDto
            {
                FilterType = filterType,
                StartDate = startDate,
                EndDate = endDate
            };

            var summary = await _financeService.GetFinanceSummaryAsync(filter);
            ViewBag.CurrentFilter = filterType;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(summary);
        }
    }
}
