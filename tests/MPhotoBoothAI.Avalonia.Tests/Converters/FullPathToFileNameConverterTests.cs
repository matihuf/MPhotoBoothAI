using MPhotoBoothAI.Avalonia.Converters;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Tests.Converters;
public class FullPathToFileNameConverterTests
{
    private FullPathToFileNameConverter _converter;

    public FullPathToFileNameConverterTests()
    {
        _converter = new();
    }

    [Fact]
    public void Convert_ReturnsFileNameWithoutExtension_WhenValidPathProvided()
    {
        // Arrange
        string path = @"C:\folder\example.txt";
        string expected = "example";

        // Act
        var result = _converter.Convert(path, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ReturnsFileNameWithoutExtension_WhenPathContainsMultipleDots()
    {
        // Arrange
        string path = @"C:\folder\my.file.name.txt";
        string expected = "my.file.name";

        // Act
        var result = _converter.Convert(path, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ReturnsFileNameWithoutExtension_WhenOnlyFileNameProvided()
    {
        // Arrange
        string path = "document.pdf";
        string expected = "document";

        // Act
        var result = _converter.Convert(path, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ReturnsEmptyString_WhenEmptyStringProvided()
    {
        // Arrange
        string path = "";
        string expected = string.Empty;

        // Act
        var result = _converter.Convert(path, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ReturnsEmptyString_WhenNullProvided()
    {
        // Arrange
        object? path = null;
        string expected = string.Empty;

        // Act
        var result = _converter.Convert(path, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ReturnsEmptyString_WhenNonStringValueProvided()
    {
        // Arrange
        int value = 123;
        string expected = string.Empty;

        // Act
        var result = _converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }
}
