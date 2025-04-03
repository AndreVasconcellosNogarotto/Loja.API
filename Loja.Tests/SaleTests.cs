using Loja.Domain.Entities;
using Loja.Domain.ValueObject;

namespace Loja.Tests;
public class SaleTests
{
    private readonly Customer _customer;
    private readonly Branch _branch;
    private readonly Product _product1;
    private readonly Product _product2;

    public SaleTests()
    {
        // Arrange - configuração comum para todos os testes
        _customer = new Customer("CUST123", "João Silva", "joao@email.com", "12345678900");
        _branch = new Branch("BR001", "Loja Central", "Av. Paulista, 1000");
        _product1 = new Product("PROD001", "Smartphone", "Smartphone XYZ", new Money(1500.00m));
        _product2 = new Product("PROD002", "Tablet", "Tablet ABC", new Money(2500.00m));
    }

    [Fact]
    public void Should_Create_Sale_With_Valid_Data()
    {
        // Act
        var sale = new Sale("20250402001", _customer, _branch);

        // Assert
        Assert.NotNull(sale);
        Assert.Equal("20250402001", sale.SaleNumber);
        Assert.Equal(_customer.Id, sale.CustomerId);
        Assert.Equal(_branch.Id, sale.BranchId);
        Assert.False(sale.Cancelled);
        Assert.Empty(sale.Items);
        Assert.Equal(0, sale.TotalAmount.Value);
    }

    [Fact]
    public void Should_Add_Item_To_Sale()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);

        // Act
        var item = sale.AddItem(_product1, 1, new Money(1500.00m));

        // Assert
        Assert.NotNull(item);
        Assert.Single(sale.Items);
        Assert.Equal(_product1.Id, item.ProductId);
        Assert.Equal(1, item.Quantity);
        Assert.Equal(1500.00m, item.UnitPrice.Value);
        Assert.Equal(0, item.DiscountPercentage); // Menos de 4 itens, sem desconto
        Assert.Equal(1500.00m, item.TotalPrice.Value);
        Assert.Equal(1500.00m, sale.TotalAmount.Value);
    }

    [Fact]
    public void Should_Apply_10_Percent_Discount_For_4_To_9_Items()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);

        // Act
        var item = sale.AddItem(_product1, 5, new Money(1500.00m));

        // Assert
        Assert.Equal(10, item.DiscountPercentage);
        Assert.Equal(6750.00m, item.TotalPrice.Value); // 5 * 1500 * 0.9 = 6750
        Assert.Equal(6750.00m, sale.TotalAmount.Value);
    }

    [Fact]
    public void Should_Apply_20_Percent_Discount_For_10_To_20_Items()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);

        // Act
        var item = sale.AddItem(_product1, 10, new Money(1500.00m));

        // Assert
        Assert.Equal(20, item.DiscountPercentage);
        Assert.Equal(12000.00m, item.TotalPrice.Value); // 10 * 1500 * 0.8 = 12000
        Assert.Equal(12000.00m, sale.TotalAmount.Value);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_More_Than_20_Items()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => sale.AddItem(_product1, 21, new Money(1500.00m)));
        Assert.Contains("Quantity cannot exceed 20 items", ex.Message);
    }

    [Fact]
    public void Should_Update_Item_Quantity()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);
        var item = sale.AddItem(_product1, 1, new Money(1500.00m));

        // Act
        sale.UpdateItem(item.Id, 5);

        // Assert
        var updatedItem = Assert.Single(sale.Items);
        Assert.Equal(5, updatedItem.Quantity);
        Assert.Equal(10, updatedItem.DiscountPercentage); // Agora tem 5 itens, deve aplicar 10% de desconto
        Assert.Equal(6750.00m, updatedItem.TotalPrice.Value); // 5 * 1500 * 0.9 = 6750
        Assert.Equal(6750.00m, sale.TotalAmount.Value);
    }

    [Fact]
    public void Should_Cancel_Sale_Item()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);
        var item1 = sale.AddItem(_product1, 1, new Money(1500.00m));
        var item2 = sale.AddItem(_product2, 1, new Money(2500.00m));

        // Act
        sale.CancelItem(item1.Id);

        // Assert
        Assert.Equal(2, sale.Items.Count);
        var cancelledItem = sale.Items.First(i => i.Id == item1.Id);
        Assert.True(cancelledItem.Cancelled);
        Assert.Equal(0, cancelledItem.TotalPrice.Value);
        Assert.Equal(2500.00m, sale.TotalAmount.Value); // Apenas item2 é contabilizado
    }

    [Fact]
    public void Should_Cancel_Sale()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);
        sale.AddItem(_product1, 1, new Money(1500.00m));
        sale.AddItem(_product2, 1, new Money(2500.00m));

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.Cancelled);
        Assert.Equal(0, sale.TotalAmount.Value);
        foreach (var item in sale.Items)
        {
            Assert.True(item.Cancelled);
            Assert.Equal(0, item.TotalPrice.Value);
        }
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Item_To_Cancelled_Sale()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);
        sale.Cancel();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => sale.AddItem(_product1, 1, new Money(1500.00m)));
        Assert.Contains("Cannot add items to a cancelled sale", ex.Message);
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_Item_In_Cancelled_Sale()
    {
        // Arrange
        var sale = new Sale("20250402001", _customer, _branch);
        var item = sale.AddItem(_product1, 1, new Money(1500.00m));
        sale.Cancel();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => sale.UpdateItem(item.Id, 5));
        Assert.Contains("Cannot update items in a cancelled sale", ex.Message);
    }
}
