using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Tests;

/// <summary>
/// Unit tests for Customer model to ensure data integrity and validation
/// </summary>
public class CustomerTests
{
    [Fact]
    public void Customer_DefaultConstructor_SetsIdToNull()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        Assert.Null(customer.Id);
    }

    [Fact]
    public void Customer_PropertiesCanBeSet()
    {
        // Arrange
        var customer = new Customer();
        var expectedName = "Test Customer";
        var expectedType = "Enterprise";
        var expectedIndustry = "Technology";

        // Act
        customer.Name = expectedName;
        customer.Type = expectedType;
        customer.Industry = expectedIndustry;

        // Assert
        Assert.Equal(expectedName, customer.Name);
        Assert.Equal(expectedType, customer.Type);
        Assert.Equal(expectedIndustry, customer.Industry);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Customer_NameCanBeEmptyOrNull(string? name)
    {
        // Arrange
        var customer = new Customer();

        // Act
        customer.Name = name;

        // Assert
        Assert.Equal(name, customer.Name);
    }

    [Fact]
    public void Customer_CanHaveAddressAssociation()
    {
        // Arrange
        var customer = new Customer();
        var address = new Address { Street = "123 Main St", City = "Test City" };

        // Act
        customer.Address = address;
        customer.AddressId = 1;

        // Assert
        Assert.Equal(address, customer.Address);
        Assert.Equal(1, customer.AddressId);
    }
}