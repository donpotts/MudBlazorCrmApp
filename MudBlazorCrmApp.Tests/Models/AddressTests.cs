using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Tests.Models;

/// <summary>
/// Unit tests for Address model to ensure proper functionality
/// </summary>
public class AddressTests
{
    [Fact]
    public void Address_DefaultConstructor_InitializesProperties()
    {
        // Arrange & Act
        var address = new Address();

        // Assert
        Assert.Null(address.Id);
        Assert.Null(address.Street);
        Assert.Null(address.City);
        Assert.Null(address.State);
        Assert.Null(address.ZipCode);
        Assert.Null(address.Country);
    }

    [Fact]
    public void Address_CanSetAllProperties()
    {
        // Arrange
        var address = new Address();
        var street = "123 Main St";
        var city = "Anytown";
        var state = "CA";
        var zipCode = 12345L;
        var country = "USA";

        // Act
        address.Street = street;
        address.City = city;
        address.State = state;
        address.ZipCode = zipCode;
        address.Country = country;

        // Assert
        Assert.Equal(street, address.Street);
        Assert.Equal(city, address.City);
        Assert.Equal(state, address.State);
        Assert.Equal(zipCode, address.ZipCode);
        Assert.Equal(country, address.Country);
    }

    [Fact]
    public void Address_ModifiedDateCanBeSet()
    {
        // Arrange
        var address = new Address();
        var modifiedDate = DateTime.Now;

        // Act
        address.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, address.ModifiedDate);
    }
}