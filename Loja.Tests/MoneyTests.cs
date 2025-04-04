using Loja.Domain.ValueObject;

namespace Loja.Tests;
public class MoneyTests
{
    [Fact]
    public void Should_Create_Money_With_Default_Currency()
    {
        // Act
        var money = new Money(100.50m);

        // Assert
        Assert.Equal(100.50m, money.Value);
        Assert.Equal("BRL", money.Currency);
    }

    [Fact]
    public void Should_Create_Money_With_Specified_Currency()
    {
        // Act
        var money = new Money(100.50m, "USD");

        // Assert
        Assert.Equal(100.50m, money.Value);
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Should_Add_Two_Money_Values_With_Same_Currency()
    {
        // Arrange
        var money1 = new Money(100.50m, "BRL");
        var money2 = new Money(50.25m, "BRL");

        // Act
        var result = money1 + money2;

        // Assert
        Assert.Equal(150.75m, result.Value);
        Assert.Equal("BRL", result.Currency);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Different_Currencies()
    {
        // Arrange
        var money1 = new Money(100.50m, "BRL");
        var money2 = new Money(50.25m, "USD");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => money1 + money2);
        Assert.Contains("Cannot add money values with different currencies", ex.Message);
    }

    [Fact]
    public void Should_Subtract_Two_Money_Values_With_Same_Currency()
    {
        // Arrange
        var money1 = new Money(100.50m, "BRL");
        var money2 = new Money(50.25m, "BRL");

        // Act
        var result = money1 - money2;

        // Assert
        Assert.Equal(50.25m, result.Value);
        Assert.Equal("BRL", result.Currency);
    }

    [Fact]
    public void Should_Throw_Exception_When_Subtracting_Different_Currencies()
    {
        // Arrange
        var money1 = new Money(100.50m, "BRL");
        var money2 = new Money(50.25m, "USD");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => money1 - money2);
        Assert.Contains("Cannot subtract money values with different currencies", ex.Message);
    }

    [Fact]
    public void Should_Multiply_Money_By_Decimal()
    {
        // Arrange
        var money = new Money(100.00m, "BRL");

        // Act
        var result = money * 2.5m;

        // Assert
        Assert.Equal(250.00m, result.Value);
        Assert.Equal("BRL", result.Currency);
    }

    [Fact]
    public void Should_Compare_Money_Values_With_Same_Currency()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(50.00m, "BRL");
        var money3 = new Money(100.00m, "BRL");

        // Act & Assert
        Assert.True(money1 > money2);
        Assert.True(money2 < money1);
        Assert.True(money1 == money3);
        Assert.True(money1 >= money3);
        Assert.True(money1 <= money3);
        Assert.False(money1 != money3);
    }

    [Fact]
    public void Should_Throw_Exception_When_Comparing_Different_Currencies()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(50.00m, "USD");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => money1.CompareTo(money2));
        Assert.Contains("Cannot compare money values with different currencies", ex.Message);
    }

    [Fact]
    public void ToString_Should_Return_Formatted_Value_And_Currency()
    {
        // Arrange
        var money = new Money(1234.56m, "BRL");

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal("1234.56 BRL", result);
    }
}
