using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Common.Tests;
using SkiaSharp;

namespace MPhotoBooth.Unit.Tests.Application.Extensions;

public class SKBitmapExtensionsTests
{
    [Theory]
    [InlineData(SKColorType.Bgra8888)]
    [InlineData(SKColorType.Rgba8888)]
    [InlineData(SKColorType.Rgb888x)]
    [InlineData(SKColorType.Rgb565)]
    public void ToMat_ShouldConvertSKBitmapToMat_WithSupportedColorType(SKColorType colorType)
    {
        // Arrange
        using var bitmap = new SKBitmap(new SKImageInfo(100, 100, colorType));

        // Act
        using var mat = bitmap.ToMat();

        // Assert
        Assert.NotNull(mat);
        Assert.Equal(100, mat.Width);
        Assert.Equal(100, mat.Height);
        Assert.Equal(4, mat.NumberOfChannels);
    }

    [Fact]
    public void ToMat_ShouldThrowException_WhenColorTypeIsUnsupported()
    {
        // Arrange
        using var bitmap = new SKBitmap(new SKImageInfo(100, 100, SKColorType.Alpha8));

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => bitmap.ToMat());
    }

    [Fact]
    public void DrawRoundedRectangle_ShouldDrawRectangle()
    {
        // Arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/roundedRectangle.dat");
        using var blackMat = Mat.Zeros(50, 50, DepthType.Cv8U, 3);
        using var bitmap = blackMat.ToSKBitmap();
        // Act
        bitmap.DrawRoundedRectangle(new SKRect(5, 5, 20, 20), 3, new SKColor(255, 0, 0), 1);
        // Assert
        using var mat = bitmap.ToMat();
        mat.Save("ttt.png");
        Assert.True(RawMatFile.RawEqual(expected, mat, 2, 2));
    }
}
