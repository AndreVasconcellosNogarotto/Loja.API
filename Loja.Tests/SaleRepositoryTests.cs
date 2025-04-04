using Loja.Domain.Entities;
using Loja.Domain.Interfaces;
using Loja.Domain.ValueObject;
using Loja.Infrastructure.Data;
using Loja.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

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
        // Arrange
        var options = GetInMemoryDbOptions();
        var customer = new Customer("CUST123", "João Silva", "joao@email.com", "12345678900");
        var branch = new Branch("BR001", "Loja Central", "Av. Paulista, 1000");
        var product = new Product("PROD001", "Smartphone", "Smartphone XYZ", new Money(1500.00m));
        var sale = new Sale("SALE001", customer, branch);
        sale.AddItem(product, 5, new Money(1500.00m));

        // Act
        using (var context = new AppDbContext(options))
        {
            context.Customers.Add(customer);
            context.Branches.Add(branch);
            context.Products.Add(product);
            context.Sales.Add(sale);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(options))
        {
            var repository = new SaleRepository(context);
            var retrievedSale = await context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            Assert.NotNull(retrievedSale);
            Assert.Equal("SALE001", retrievedSale.SaleNumber);
            Assert.Single(retrievedSale.Items);

            var item = retrievedSale.Items.First();
            Assert.Equal(5, item.Quantity);
            Assert.Equal(10, item.DiscountPercentage);
        }
    }

    [Fact]
    public async Task Should_Cancel_Sale()
    {
        var options = GetInMemoryDbOptions();
        var customer = new Customer("CUST123", "João Silva", "joao@email.com", "12345678900");
        var branch = new Branch("BR001", "Loja Central", "Av. Paulista, 1000");
        var product = new Product("PROD001", "Smartphone", "Smartphone XYZ", new Money(1500.00m));
        var sale = new Sale("SALE001", customer, branch);
        sale.AddItem(product, 1, new Money(1500.00m));

        // Act
        using (var context = new AppDbContext(options))
        {
            context.Customers.Add(customer);
            context.Branches.Add(branch);
            context.Products.Add(product);
            context.Sales.Add(sale);
            await context.SaveChangesAsync();

            // Cancelar a venda
            sale.Cancel();
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(options))
        {
            var cancelledSale = await context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            Assert.NotNull(cancelledSale);
            Assert.True(cancelledSale.Cancelled);

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
        var mockRepository = new Mock<ISaleRepository>();
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        // var options = GetInMemoryDbOptions();

        mockRepository.SetupSequence(repo => repo.GenerateSaleNumberAsync())
       .ReturnsAsync($"{today}000001")
       .ReturnsAsync($"{today}000002");

        mockRepository.SetupSequence(repo => repo.GenerateSaleNumberAsync())
          .ReturnsAsync($"{today}000001")
          .ReturnsAsync($"{today}000002");

        // Act
        var saleNumber1 = mockRepository.Object.GenerateSaleNumberAsync().Result;
        var saleNumber2 = mockRepository.Object.GenerateSaleNumberAsync().Result;

        // Assert
        Assert.NotEqual(saleNumber1, saleNumber2);
    }
}
