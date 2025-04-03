using Microsoft.EntityFrameworkCore;
using Loja.Domain.Entities;
using Loja.Infrastructure.Data.EntityConfiguration;

namespace Loja.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
            modelBuilder.ApplyConfiguration(new SaleItemConfiguration());
            modelBuilder.Entity<SaleItem>()
                .OwnsOne(si => si.UnitPrice, ownedBuilder =>
                        {
                            ownedBuilder.Property(m => m.Value).HasColumnName("UnitPrice");
                            ownedBuilder.Property(m => m.Currency).HasColumnName("Currency");
                        });
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetUpdatedAt();
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
