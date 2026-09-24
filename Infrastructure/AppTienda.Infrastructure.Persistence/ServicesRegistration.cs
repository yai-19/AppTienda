using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Enums;
using AppTienda.Core.Domain.Interfaces;
using AppTienda.Infrastructure.Persistence.Contexts;
using AppTienda.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppTienda.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            var provider = config.GetValue<string>("DatabaseProvider") ?? "Sqlite";

            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<AppTiendaDbContext>(opt => opt.UseInMemoryDatabase("AppTiendaInMemoryDb"));
            }
            else if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<AppTiendaDbContext>(opt =>
                    opt.UseSqlServer(connectionString,
                        m => m.MigrationsAssembly(typeof(AppTiendaDbContext).Assembly.FullName)),
                    ServiceLifetime.Transient);
            }
            else
            {
                // Default: SQLite (portable and zero-configuration)
                var sqliteConn = config.GetConnectionString("SqliteConnection") ?? "Data Source=AppTienda.db";
                services.AddDbContext<AppTiendaDbContext>(opt =>
                    opt.UseSqlite(sqliteConn,
                        m => m.MigrationsAssembly(typeof(AppTiendaDbContext).Assembly.FullName)),
                    ServiceLifetime.Transient);
            }
            #endregion

            #region Repositories IOC
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            #endregion
        }

        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppTiendaDbContext>();

            await context.Database.EnsureCreatedAsync();

            if (!await context.Categories.AnyAsync())
            {
                var catRopa = new Category { Name = "Ropa y Calzado", Description = "Prendas de vestir, zapatos y accesorios de moda" };
                var catElectro = new Category { Name = "Electrónica y Accesorios", Description = "Dispositivos, cargadores, cables y gadgets" };
                var catHogar = new Category { Name = "Hogar y Cocina", Description = "Artículos para el hogar y utensilios de cocina" };
                var catBelleza = new Category { Name = "Belleza y Cuidado Personal", Description = "Cosméticos, perfumes y cuidado de la piel" };

                await context.Categories.AddRangeAsync(catRopa, catElectro, catHogar, catBelleza);
                await context.SaveChangesAsync();

                var p1 = new Product
                {
                    Name = "Auriculares Bluetooth Pro",
                    Description = "Auriculares inalámbricos con cancelación de ruido",
                    Sku = "ELEC-001",
                    PurchasePrice = 15.00m,
                    SalePrice = 35.00m,
                    Stock = 8,
                    Status = ProductStatus.Disponible,
                    CategoryId = catElectro.Id,
                    CreatedAt = DateTime.Now.AddDays(-20)
                };

                var p2 = new Product
                {
                    Name = "Camiseta Algodón Premium",
                    Description = "Camiseta casual 100% algodón",
                    Sku = "ROPA-101",
                    PurchasePrice = 8.00m,
                    SalePrice = 20.00m,
                    Stock = 15,
                    Status = ProductStatus.Disponible,
                    CategoryId = catRopa.Id,
                    CreatedAt = DateTime.Now.AddDays(-15)
                };

                var p3 = new Product
                {
                    Name = "Botella Térmica Inox 750ml",
                    Description = "Botella para agua fría/caliente 24 horas",
                    Sku = "HOG-301",
                    PurchasePrice = 6.50m,
                    SalePrice = 16.00m,
                    Stock = 2, // Low stock
                    Status = ProductStatus.Disponible,
                    CategoryId = catHogar.Id,
                    CreatedAt = DateTime.Now.AddDays(-10)
                };

                var p4 = new Product
                {
                    Name = "Perfume Elegance 100ml",
                    Description = "Fragancia de larga duración",
                    Sku = "BEL-501",
                    PurchasePrice = 22.00m,
                    SalePrice = 50.00m,
                    Stock = 0,
                    Status = ProductStatus.Vendido,
                    CategoryId = catBelleza.Id,
                    CreatedAt = DateTime.Now.AddDays(-30)
                };

                await context.Products.AddRangeAsync(p1, p2, p3, p4);
                await context.SaveChangesAsync();

                // Initial Sales Examples
                var s1 = new Sale
                {
                    SaleDate = DateTime.Now.AddDays(-5),
                    ProductId = p1.Id,
                    Quantity = 2,
                    UnitCost = 15.00m,
                    UnitPrice = 35.00m,
                    TotalCost = 30.00m,
                    TotalAmount = 70.00m,
                    Profit = 40.00m,
                    PaymentMethod = PaymentMethod.Transferencia,
                    CustomerName = "Carlos Pérez",
                    Notes = "Venta por catálogo"
                };

                var s2 = new Sale
                {
                    SaleDate = DateTime.Now.AddDays(-2),
                    ProductId = p2.Id,
                    Quantity = 3,
                    UnitCost = 8.00m,
                    UnitPrice = 20.00m,
                    TotalCost = 24.00m,
                    TotalAmount = 60.00m,
                    Profit = 36.00m,
                    PaymentMethod = PaymentMethod.Efectivo,
                    CustomerName = "María Gómez",
                    Notes = "Cliente frecuente"
                };

                var s3 = new Sale
                {
                    SaleDate = DateTime.Now.AddDays(-1),
                    ProductId = p4.Id,
                    Quantity = 1,
                    UnitCost = 22.00m,
                    UnitPrice = 50.00m,
                    TotalCost = 22.00m,
                    TotalAmount = 50.00m,
                    Profit = 28.00m,
                    PaymentMethod = PaymentMethod.Tarjeta,
                    CustomerName = "Laura Rivas"
                };

                await context.Sales.AddRangeAsync(s1, s2, s3);

                // Initial Expenses Examples
                var e1 = new Expense
                {
                    ExpenseDate = DateTime.Now.AddDays(-10),
                    Category = ExpenseCategory.Alquiler,
                    Description = "Pago mensual del local / depósito",
                    Amount = 50.00m,
                    Notes = "Comprobante 001"
                };

                var e2 = new Expense
                {
                    ExpenseDate = DateTime.Now.AddDays(-6),
                    Category = ExpenseCategory.Transporte,
                    Description = "Envío y flete de mercadería",
                    Amount = 15.00m
                };

                var e3 = new Expense
                {
                    ExpenseDate = DateTime.Now.AddDays(-3),
                    Category = ExpenseCategory.Publicidad,
                    Description = "Campaña en redes sociales",
                    Amount = 20.00m
                };

                await context.Expenses.AddRangeAsync(e1, e2, e3);
                await context.SaveChangesAsync();
            }
        }
    }
}
