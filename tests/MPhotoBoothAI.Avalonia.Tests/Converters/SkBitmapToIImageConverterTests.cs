using Avalonia.Data;
using Avalonia.Media.Imaging;
using MPhotoBoothAI.Avalonia.Converters;
using SkiaSharp;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Tests.Converters
{
    public class SkBitmapToIImageConverterTests
    {
        private readonly SkBitmapToIImageConverter _converter;

        public SkBitmapToIImageConverterTests()
        {
            _converter = new SkBitmapToIImageConverter();
        }

        [Fact]
        public void Convert_ShouldReturnNull_WhenValueIsNull()
        {
            // Act
            var result = _converter.Convert(null, typeof(WriteableBitmap), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Convert_ShouldReturnBindingNotification_WhenTargetTypeIsNotWriteableBitmap()
        {
            // Act
            var result = _converter.Convert(new SKBitmap(), typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            var bindingNotification = Assert.IsType<BindingNotification>(result);
            Assert.IsType<NotSupportedException>(bindingNotification.Error);
            Assert.Equal(BindingErrorType.Error, bindingNotification.ErrorType);
        }
    }
}
