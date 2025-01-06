using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Common.Tests;
using SkiaSharp;
using System.Drawing;

namespace MPhotoBooth.Unit.Tests.Application.Extensions;

public class MatExtensionsTests
{
    [Fact]
    public void ToSKBitmap_ShouldConvertMatToSKBitmap_With4Channels()
    {
        // Arrange
        using var mat = new Mat(100, 100, DepthType.Cv8U, 4);

        // Act
        using var skBitmap = mat.ToSKBitmap();

        // Assert
        Assert.NotNull(skBitmap);
        Assert.IsType<SKBitmap>(skBitmap);
        Assert.Equal(100, skBitmap.Width);
        Assert.Equal(100, skBitmap.Height);
        Assert.Equal(SKColorType.Bgra8888, skBitmap.ColorType);
    }

    [Fact]
    public void ToSKBitmap_ShouldConvertMatToSKBitmap_With3Channels()
    {
        // Arrange
        using var mat = new Mat(100, 100, DepthType.Cv8U, 3);

        // Act
        using var skBitmap = mat.ToSKBitmap();

        // Assert
        Assert.NotNull(skBitmap);
        Assert.IsType<SKBitmap>(skBitmap);
        Assert.Equal(100, skBitmap.Width);
        Assert.Equal(100, skBitmap.Height);
        Assert.Equal(SKColorType.Bgra8888, skBitmap.ColorType);
    }

    [Fact]
    public void ToSKBitmap_ShouldThrowException_WhenMatDepthIsNotCv8U()
    {
        // Arrange
        using var mat = new Mat(100, 100, DepthType.Cv16U, 4);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => mat.ToSKBitmap());
    }

    [Fact]
    public void ToSKBitmap_ShouldThrowException_WhenMatHasUnsupportedChannelCount()
    {
        // Arrange
        using var mat = new Mat(100, 100, DepthType.Cv8U, 2);  // Invalid channel count for conversion

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => mat.ToSKBitmap());
    }

    [Fact]
    public void DrawRoundedRectangle_ShouldDrawRectangle()
    {
        // Arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/roundedRectangle.dat");
        using var mat = Mat.Zeros(50, 50, DepthType.Cv8U, 3);
        // Act
        mat.DrawRoundedRectangle(new Rectangle(5, 5, 20, 20), 3, new MCvScalar(0, 0, 255), 1);
        // Assert
        Assert.True(RawMatFile.RawEqual(expected, mat));
    }
}
