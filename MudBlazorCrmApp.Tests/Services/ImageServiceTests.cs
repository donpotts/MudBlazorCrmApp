using MudBlazorCrmApp.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace MudBlazorCrmApp.Tests.Services;

/// <summary>
/// Unit tests for ImageService to ensure proper image handling and validation
/// </summary>
public class ImageServiceTests
{
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly ImageService _imageService;

    public ImageServiceTests()
    {
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockEnvironment.Setup(x => x.WebRootPath).Returns("/test/path");
        _imageService = new ImageService(_mockEnvironment.Object);
    }

    [Fact]
    public void GetImageFromDataUri_ValidPngDataUri_ReturnsCorrectExtensionAndData()
    {
        // Arrange
        var testData = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==";
        var dataUri = $"data:image/png;base64,{testData}";

        // Act
        var (extension, data) = ImageService.GetImageFromDataUri(dataUri);

        // Assert
        Assert.Equal(".png", extension);
        Assert.NotEmpty(data);
        Assert.True(data.Length > 0);
    }

    [Fact]
    public void GetImageFromDataUri_ValidJpegDataUri_ReturnsCorrectExtensionAndData()
    {
        // Arrange
        var testData = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==";
        var dataUri = $"data:image/jpeg;base64,{testData}";

        // Act
        var (extension, data) = ImageService.GetImageFromDataUri(dataUri);

        // Assert
        Assert.Equal(".jpg", extension);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void GetImageFromDataUri_InvalidFormat_ThrowsArgumentException()
    {
        // Arrange
        var invalidDataUri = "invalid-data-uri";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            ImageService.GetImageFromDataUri(invalidDataUri));
        Assert.Equal("dataUri", exception.ParamName);
        Assert.Contains("Data URI format is invalid", exception.Message);
    }

    [Fact]
    public void GetImageFromDataUri_UnsupportedImageType_ThrowsArgumentException()
    {
        // Arrange
        var testData = "test-data";
        var unsupportedDataUri = $"data:image/gif;base64,{testData}";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            ImageService.GetImageFromDataUri(unsupportedDataUri));
        Assert.Equal("dataUri", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("data:image/png;base64")]
    [InlineData("test,")]
    public void GetImageFromDataUri_MalformedDataUri_ThrowsArgumentException(string malformedUri)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            ImageService.GetImageFromDataUri(malformedUri));
    }
}