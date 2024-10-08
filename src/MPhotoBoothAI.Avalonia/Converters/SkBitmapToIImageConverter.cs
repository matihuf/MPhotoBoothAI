using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using MPhotoBoothAI.Avalonia.Extensions;
using SkiaSharp;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters
{
    public class SkBitmapToIImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (!targetType.IsAssignableFrom(typeof(WriteableBitmap)))
            {
                return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
            }
            if (value is SKBitmap bitmap)
            {
                return bitmap.ToAvaloniaImage();
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
