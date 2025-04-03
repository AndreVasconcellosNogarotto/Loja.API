using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Loja.Domain.Entities;

namespace Loja.Infrastructure.Data.EntityConfiguration
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.DiscountPercentage)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(si => si.Cancelled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(si => si.CreatedAt)
                .IsRequired();

            builder.Property(si => si.UpdatedAt);

            builder.OwnsOne(si => si.UnitPrice, money =>
            {
                money.Property(m => m.Value)
                    .HasColumnName("UnitPrice")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasDefaultValue("BRL");
            });

            builder.HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(si => si.SaleId)
                .IsRequired();
        }
    }
}
