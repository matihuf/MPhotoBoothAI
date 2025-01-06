using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Extensions;
using SkiaSharp;

namespace MPhotoBoothAI.Avalonia.Tests.Extensions
{
    public class ImageExtensionsTests
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
    }
}
