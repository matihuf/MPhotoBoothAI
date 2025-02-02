using MPhotoBoothAI.Avalonia.Converters;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Tests.Converters;
public class BoolToOpacityLevelConverterTests
{
    private readonly BoolToOpacityLevelConverter _converter;

    public BoolToOpacityLevelConverterTests()
    {
        _converter = new BoolToOpacityLevelConverter();
    }

    [Fact]
    public void Convert_ReturnsOpacity_WhenValueIsTrueAndParameterIsDouble()
    {
        // Arrange
        bool value = true;
        double expectedOpacity = 0.5;
        object parameter = expectedOpacity;

        // Act
        var result = _converter.Convert(value, typeof(double), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedOpacity, result);
    }

    [Fact]
    public void Convert_ReturnsDefaultValue_WhenValueIsTrueButParameterIsNotDouble()
    {
        // Arrange
        bool value = true;
        object parameter = "0.5";

        // Act
        var result = _converter.Convert(value, typeof(double), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Convert_ReturnsDefaultValue_WhenValueIsFalseEvenIfParameterIsDouble()
    {
        // Arrange
        bool value = false;
        double parameter = 0.5;

        // Act
        var result = _converter.Convert(value, typeof(double), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Convert_ReturnsDefaultValue_WhenValueIsNull()
    {
        // Arrange
        object value = null;
        double parameter = 0.5;

        // Act
        var result = _converter.Convert(value, typeof(double), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Convert_ReturnsDefaultValue_WhenParameterIsNull()
    {
        // Arrange
        bool value = true;
        object parameter = null;

        // Act
        var result = _converter.Convert(value, typeof(double), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Convert_ReturnsDefaultValue_WhenValueIsNotBool()
    {
        // Arrange
        string value = "true";
        double parameter = 0.5;

        // Act
        var result = _converter.Convert(value, typeof(double), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(1, result);
    }
}
