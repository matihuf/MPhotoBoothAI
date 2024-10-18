using MPhotoBoothAI.Avalonia.Converters;

namespace MPhotoBoothAI.Avalonia.Tests.Converters;
public class TranslateConverterTests
{
    [Fact]
    public void Convert_KeyFounded_ReturnTranslated()
    {
        //arrange
        var converter = new TranslateConverter();
        string key = "pl-PL";
        //act
        string result = (string)converter.Convert(key, null, null, null);
        //assert
        Assert.NotEqual(key, result);
    }

    [Fact]
    public void Convert_KeyNotFounded_ReturnKey()
    {
        //arrange
        var converter = new TranslateConverter();
        string key = "plASFVDADPL";
        //act
        string result = (string)converter.Convert(key, null, null, null);
        //assert
        Assert.Equal(key, result);
    }
}
