using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Loja.Domain.Entities;

namespace Loja.Infrastructure.Data.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            Microsoft.EntityFrameworkCore.RelationalEntityTypeBuilderExtensions.ToTable(builder, "Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ExternalId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt);

            builder.OwnsOne(p => p.BasePrice, money =>
            {
                money.Property(m => m.Value).HasColumnName("BasePrice");
                money.Property(m => m.Currency).HasColumnName("Currency");
            });

            builder.HasIndex(p => p.ExternalId)
                .IsUnique();
        }

    }
}
