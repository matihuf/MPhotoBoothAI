using MPhotoBoothAI.Avalonia.Converters;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Tests.Converters;
public class IntCompareToBoolConverterTests
{
    private IntCompareToBoolConverter _converter;

    public IntCompareToBoolConverterTests()
    {
        _converter = new();
    }

    [Fact]
    public void Convert_ReturnsTrue_WhenIntegersAreEqual()
    {
        // Arrange
        IList<object?> values = new List<object?> { 10, 10 };
        bool expected = true;

        // Act
        var result = _converter.Convert(values, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ReturnsFalse_WhenIntegersAreNotEqual()
    {
        // Arrange
        IList<object?> values = new List<object?> { 10, 20 };
        bool expected = false;

        // Act
        var result = _converter.Convert(values, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, 10)]
    [InlineData("true", 10)]
    [InlineData("10", 10)]
    [InlineData(1, "10")]
    public void Convert_ReturnsFalse_WhenAnyValueIsNotInt(object firstValue, object secondValue)
    {
        // Arrange
        IList<object?> values = new List<object?> { firstValue, secondValue };
        bool expected = false;

        // Act
        var result = _converter.Convert(values, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

}
