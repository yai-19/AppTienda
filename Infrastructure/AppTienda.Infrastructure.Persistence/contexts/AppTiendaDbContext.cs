using AppTienda.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppTienda.Infrastructure.Persistence.Contexts
{
    public class AppTiendaDbContext : DbContext
    {
        public AppTiendaDbContext(DbContextOptions<AppTiendaDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<Expense> Expenses => Set<Expense>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
