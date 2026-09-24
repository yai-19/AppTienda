using AppTienda.Core.Application.Interfaces;
using AppTienda.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppTienda.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Services IOC
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<IFinanceService, FinanceService>();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion
        }
    }
}
