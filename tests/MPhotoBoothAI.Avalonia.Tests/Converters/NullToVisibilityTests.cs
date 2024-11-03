using MPhotoBoothAI.Avalonia.Converters;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Tests.Converters
{
    public class NullToVisibilityTests
    {
        private readonly NullToVisibility _converter;

        public NullToVisibilityTests()
        {
            _converter = new NullToVisibility();
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("NotNullString", true)]
        [InlineData(123, true)]
        public void Convert_ShouldReturnExpectedVisibility(object? input, bool expected)
        {
            // Act
            var result = _converter.Convert(input, typeof(bool), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            // Act & Assert
            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(true, typeof(object), null, CultureInfo.InvariantCulture));
        }
    }
}
