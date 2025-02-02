using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Avalonia.Converters;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Tests.Converters;
public class FormatTypeToTranslatedNameConverterTests
{
    private readonly string _notFoundFormat = "Format Not Found";

    private FormatTypeToTranslatedNameConverter _converter;

    public FormatTypeToTranslatedNameConverterTests()
    {
        _converter = new();
    }

    [Theory]
    [InlineData(FormatTypes.Stripe)]
    [InlineData(FormatTypes.PostCard)]
    public void Convert_AddExistedFormat_ReturnTanslatedValue(FormatTypes formatType)
    {
        //Act
        var result = _converter.Convert((int)formatType, null, null, CultureInfo.InvariantCulture);

        //Assert
        Assert.NotEqual(result, _notFoundFormat);
    }

    [Fact]
    public void Convert_AddNotExistingFormat_ReturnNotFound()
    {
        //Act
        var result = _converter.Convert(100, null, null, CultureInfo.InvariantCulture);

        //Assert
        Assert.Equal(result, _notFoundFormat);
    }

    [Theory]
    [InlineData("")]
    [InlineData(true)]
    [InlineData(0x200)]
    public void Convert_NotParsableObjects_ReturnNotFound(object obj)
    {
        //Act
        var result = _converter.Convert(obj, null, null, CultureInfo.InvariantCulture);

        //Assert
        Assert.Equal(result, _notFoundFormat);
    }
}
