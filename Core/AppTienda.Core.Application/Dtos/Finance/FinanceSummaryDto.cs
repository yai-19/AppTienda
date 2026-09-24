using AppTienda.Core.Application.Dtos.Expense;
using AppTienda.Core.Application.Dtos.Sale;

namespace AppTienda.Core.Application.Dtos.Finance
{
    public class FinanceSummaryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FilterTitle { get; set; } = string.Empty;

        // Key Financial Metrics
        public decimal TotalSalesRevenue { get; set; } // Total Ingresos por Ventas
        public decimal TotalCostOfGoodsSold { get; set; } // Costo de Mercancía Vendida
        public decimal GrossProfit { get; set; } // Ganancia Bruta = Ingresos - Costo Mercancía
        public decimal TotalExpenses { get; set; } // Gastos Operativos (Alquiler, Luz, etc.)
        public decimal NetProfit { get; set; } // Ganancia Neta = Ganancia Bruta - Gastos
        public decimal ProfitMarginPercentage { get; set; } // Margen de Ganancia Neta %

        // Counts & Overview
        public int TotalSalesCount { get; set; }
        public int TotalItemsSold { get; set; }
        public int TotalExpensesCount { get; set; }
        public int TotalActiveProducts { get; set; }
        public int TotalStockUnits { get; set; }
        public decimal TotalInventoryValueAtCost { get; set; }
        public decimal TotalInventoryValueAtSale { get; set; }

        // Lists for details
        public List<SaleDto> Sales { get; set; } = new List<SaleDto>();
        public List<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>();

        // Analytics Data for Charts
        public List<string> ChartLabels { get; set; } = new List<string>();
        public List<decimal> ChartSalesData { get; set; } = new List<decimal>();
        public List<decimal> ChartExpensesData { get; set; } = new List<decimal>();
        public List<decimal> ChartNetProfitData { get; set; } = new List<decimal>();

        // Expenses by Category breakdown
        public Dictionary<string, decimal> ExpensesByCategory { get; set; } = new Dictionary<string, decimal>();

        // Top Selling Products
        public List<TopProductDto> TopSellingProducts { get; set; } = new List<TopProductDto>();
    }

    public class TopProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
