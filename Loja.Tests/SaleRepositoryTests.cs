using Loja.Domain.Entities;
using Loja.Domain.ValueObject;
using Loja.Infrastructure.Data;
using Loja.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Loja.Tests;
public class SaleRepositoryTests
{
    private DbContextOptions<AppDbContext> GetInMemoryDbOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Should_Add_And_Retrieve_Sale()
    {
        var options = GetInMemoryDbOptions();
        var customer = new Customer("CUST123", "João Silva", "joao@email.com", "12345678900");
        var branch = new Branch("BR001", "Loja Central", "Av. Paulista, 1000");
        var product = new Product("PROD001", "Smartphone", "Smartphone XYZ", new Money(1500.00m));
        var sale = new Sale("SALE001", customer, branch);

        using (var context = new AppDbContext(options))
        {
            context.Customers.Add(customer);
            context.Branches.Add(branch);
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            sale.AddItem(product, 5, new Money(1500.00m));
            await repository.AddAsync(sale);
            await repository.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            var retrievedSale = await repository.GetSaleWithDetailsAsync(sale.Id);

            Assert.NotNull(retrievedSale);
            Assert.Equal("SALE001", retrievedSale.SaleNumber);
            Assert.Equal(customer.Id, retrievedSale.CustomerId);
            Assert.Equal(branch.Id, retrievedSale.BranchId);
            Assert.Single(retrievedSale.Items);

            var item = retrievedSale.Items.First();
            Assert.Equal(5, item.Quantity);
            Assert.Equal(1500.00m, item.UnitPrice.Value);
            Assert.Equal(10, item.DiscountPercentage); // 5 itens = 10% de desconto
            Assert.Equal(6750.00m, item.TotalPrice.Value); // 5 * 1500 * 0.9 = 6750
        }
    }

    [Fact]
    public async Task Should_Cancel_Sale()
    {
        // Arrange
        var options = GetInMemoryDbOptions();
        var customer = new Customer("CUST123", "João Silva", "joao@email.com", "12345678900");
        var branch = new Branch("BR001", "Loja Central", "Av. Paulista, 1000");
        var product = new Product("PROD001", "Smartphone", "Smartphone XYZ", new Money(1500.00m));
        var sale = new Sale("SALE001", customer, branch);

        using (var context = new AppDbContext(options))
        {
            context.Customers.Add(customer);
            context.Branches.Add(branch);
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            sale.AddItem(product, 1, new Money(1500.00m));
            await repository.AddAsync(sale);
            await repository.SaveChangesAsync();
        }

        // Act
        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            var retrievedSale = await repository.GetByIdAsync(sale.Id);
            retrievedSale.Cancel();
            await repository.UpdateAsync(retrievedSale);
            await repository.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            var cancelledSale = await repository.GetSaleWithDetailsAsync(sale.Id);

            Assert.NotNull(cancelledSale);
            Assert.True(cancelledSale.Cancelled);
            Assert.Equal(0, cancelledSale.TotalAmount.Value);

            foreach (var item in cancelledSale.Items)
            {
                Assert.True(item.Cancelled);
            }
        }
    }

    [Fact]
    public async Task Should_Generate_Unique_SaleNumber()
    {
        // Arrange
        var options = GetInMemoryDbOptions();

        // Act
        string saleNumber1;
        string saleNumber2;

        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            saleNumber1 = await repository.GenerateSaleNumberAsync();
        }

        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            saleNumber2 = await repository.GenerateSaleNumberAsync();
        }

        // Assert
        Assert.NotEqual(saleNumber1, saleNumber2);
        Assert.StartsWith(DateTime.UtcNow.ToString("yyyyMMdd"), saleNumber1);
        Assert.StartsWith(DateTime.UtcNow.ToString("yyyyMMdd"), saleNumber2);
    }
}
