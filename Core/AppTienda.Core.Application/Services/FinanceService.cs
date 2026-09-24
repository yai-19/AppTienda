using AutoMapper;
using AppTienda.Core.Application.Dtos.Expense;
using AppTienda.Core.Application.Dtos.Finance;
using AppTienda.Core.Application.Dtos.Sale;
using AppTienda.Core.Application.Interfaces;
using AppTienda.Core.Domain.Interfaces;
using System.Globalization;

namespace AppTienda.Core.Application.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public FinanceService(
            ISaleRepository saleRepository,
            IExpenseRepository expenseRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _expenseRepository = expenseRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<FinanceSummaryDto> GetFinanceSummaryAsync(FinanceFilterDto filter)
        {
            var now = DateTime.Now;
            DateTime startDate;
            DateTime endDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            string title;

            switch (filter.FilterType)
            {
                case DateFilterType.Hoy:
                    startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    title = "Hoy (" + now.ToString("dd/MM/yyyy") + ")";
                    break;
                case DateFilterType.EstaSemana:
                    int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = now.AddDays(-1 * diff).Date;
                    title = "Esta Semana (" + startDate.ToString("dd/MM") + " - " + now.ToString("dd/MM/yyyy") + ")";
                    break;
                case DateFilterType.EsteMes:
                    startDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                    title = "Este Mes (" + now.ToString("MMMM yyyy", new CultureInfo("es-ES")) + ")";
                    break;
                case DateFilterType.EsteAno:
                    startDate = new DateTime(now.Year, 1, 1, 0, 0, 0);
                    title = "Este Año (" + now.Year + ")";
                    break;
                case DateFilterType.Historico:
                    startDate = new DateTime(2000, 1, 1);
                    title = "Histórico Total";
                    break;
                case DateFilterType.Personalizado:
                    startDate = filter.StartDate ?? new DateTime(now.Year, now.Month, 1);
                    endDate = filter.EndDate.HasValue 
                        ? new DateTime(filter.EndDate.Value.Year, filter.EndDate.Value.Month, filter.EndDate.Value.Day, 23, 59, 59)
                        : new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                    title = "Personalizado (" + startDate.ToString("dd/MM/yyyy") + " al " + endDate.ToString("dd/MM/yyyy") + ")";
                    break;
                default:
                    startDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                    title = "Este Mes";
                    break;
            }

            var salesEntities = await _saleRepository.GetSalesByDateRangeAsync(startDate, endDate);
            var expensesEntities = await _expenseRepository.GetExpensesByDateRangeAsync(startDate, endDate);
            var allProducts = await _productRepository.GetProductsWithCategoryAsync();

            var sales = _mapper.Map<List<SaleDto>>(salesEntities.OrderByDescending(s => s.SaleDate).ToList());
            var expenses = _mapper.Map<List<ExpenseDto>>(expensesEntities.OrderByDescending(e => e.ExpenseDate).ToList());

            var totalSalesRevenue = sales.Sum(s => s.TotalAmount);
            var totalCostOfGoodsSold = sales.Sum(s => s.TotalCost);
            var grossProfit = totalSalesRevenue - totalCostOfGoodsSold;
            var totalExpenses = expenses.Sum(e => e.Amount);
            var netProfit = grossProfit - totalExpenses;
            var profitMargin = totalSalesRevenue > 0 ? (netProfit / totalSalesRevenue) * 100 : 0;

            // Products metrics
            var activeProducts = allProducts.Where(p => p.Stock > 0).ToList();
            var totalStockUnits = allProducts.Sum(p => p.Stock);
            var totalInventoryValueAtCost = allProducts.Sum(p => p.Stock * p.PurchasePrice);
            var totalInventoryValueAtSale = allProducts.Sum(p => p.Stock * p.SalePrice);

            // Expenses by Category
            var expensesByCategory = expenses
                .GroupBy(e => e.Category.ToString())
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            // Top selling products
            var topProducts = sales
                .GroupBy(s => s.ProductName ?? "Producto")
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.TotalAmount),
                    TotalProfit = g.Sum(x => x.Profit)
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(5)
                .ToList();

            // Monthly / Weekly chart data
            var chartLabels = new List<string>();
            var chartSalesData = new List<decimal>();
            var chartExpensesData = new List<decimal>();
            var chartNetProfitData = new List<decimal>();

            // Group by month for longer periods or by day for shorter periods
            if ((endDate - startDate).TotalDays <= 31)
            {
                // Day by day
                for (var d = startDate.Date; d <= endDate.Date; d = d.AddDays(1))
                {
                    var dayLabel = d.ToString("dd/MM");
                    var daySales = sales.Where(s => s.SaleDate.Date == d).Sum(s => s.TotalAmount);
                    var dayCosts = sales.Where(s => s.SaleDate.Date == d).Sum(s => s.TotalCost);
                    var dayExpenses = expenses.Where(e => e.ExpenseDate.Date == d).Sum(e => e.Amount);
                    var dayGross = daySales - dayCosts;
                    var dayNet = dayGross - dayExpenses;

                    chartLabels.Add(dayLabel);
                    chartSalesData.Add(daySales);
                    chartExpensesData.Add(dayExpenses);
                    chartNetProfitData.Add(dayNet);
                }
            }
            else
            {
                // Month by month
                var currentMonth = new DateTime(startDate.Year, startDate.Month, 1);
                var endMonth = new DateTime(endDate.Year, endDate.Month, 1);

                while (currentMonth <= endMonth)
                {
                    var label = currentMonth.ToString("MMM yyyy", new CultureInfo("es-ES"));
                    var monthSales = sales.Where(s => s.SaleDate.Year == currentMonth.Year && s.SaleDate.Month == currentMonth.Month).Sum(s => s.TotalAmount);
                    var monthCosts = sales.Where(s => s.SaleDate.Year == currentMonth.Year && s.SaleDate.Month == currentMonth.Month).Sum(s => s.TotalCost);
                    var monthExpenses = expenses.Where(e => e.ExpenseDate.Year == currentMonth.Year && e.ExpenseDate.Month == currentMonth.Month).Sum(e => e.Amount);
                    var monthGross = monthSales - monthCosts;
                    var monthNet = monthGross - monthExpenses;

                    chartLabels.Add(label);
                    chartSalesData.Add(monthSales);
                    chartExpensesData.Add(monthExpenses);
                    chartNetProfitData.Add(monthNet);

                    currentMonth = currentMonth.AddMonths(1);
                }
            }

            return new FinanceSummaryDto
            {
                StartDate = startDate,
                EndDate = endDate,
                FilterTitle = title,
                TotalSalesRevenue = totalSalesRevenue,
                TotalCostOfGoodsSold = totalCostOfGoodsSold,
                GrossProfit = grossProfit,
                TotalExpenses = totalExpenses,
                NetProfit = netProfit,
                ProfitMarginPercentage = Math.Round(profitMargin, 2),
                TotalSalesCount = sales.Count,
                TotalItemsSold = sales.Sum(s => s.Quantity),
                TotalExpensesCount = expenses.Count,
                TotalActiveProducts = activeProducts.Count,
                TotalStockUnits = totalStockUnits,
                TotalInventoryValueAtCost = totalInventoryValueAtCost,
                TotalInventoryValueAtSale = totalInventoryValueAtSale,
                Sales = sales,
                Expenses = expenses,
                ChartLabels = chartLabels,
                ChartSalesData = chartSalesData,
                ChartExpensesData = chartExpensesData,
                ChartNetProfitData = chartNetProfitData,
                ExpensesByCategory = expensesByCategory,
                TopSellingProducts = topProducts
            };
        }
    }
}
