using AppTienda.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppTienda.Infrastructure.Persistence.EntitiesConfigurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ExpenseDate).IsRequired();
            builder.Property(e => e.Category).IsRequired();
            builder.Property(e => e.Description).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Notes).HasMaxLength(300);
        }
    }
}
