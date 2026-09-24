using AppTienda.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppTienda.Infrastructure.Persistence.EntitiesConfigurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.SaleDate).IsRequired();
            builder.Property(s => s.Quantity).IsRequired();
            builder.Property(s => s.UnitCost).HasColumnType("decimal(18,2)");
            builder.Property(s => s.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(s => s.TotalCost).HasColumnType("decimal(18,2)");
            builder.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Property(s => s.Profit).HasColumnType("decimal(18,2)");
            builder.Property(s => s.PaymentMethod).IsRequired();
            builder.Property(s => s.CustomerName).HasMaxLength(100);
            builder.Property(s => s.Notes).HasMaxLength(300);

            builder.HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
