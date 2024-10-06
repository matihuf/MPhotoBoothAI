using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Emgu.CV;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;

public class MatConverter : IValueConverter
{
    public static readonly MatConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        if (!targetType.IsAssignableFrom(typeof(WriteableBitmap)))
            return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);

        var mat = value switch
        {
            Mat mata => mata,
            _ => null,
        };
        if (mat == null) return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
        if (parameter is WriteableBitmap wb)
        {
            try
            {
                mat.ToBitmapParallel(wb);
                return wb;
            }
            catch (ArgumentException)
            {
                // ignored
            }
        }
        var wbx = new WriteableBitmap(new PixelSize(mat.Width, mat.Height), new Vector(96, 96));
        mat.ToBitmapParallel(wbx);
        return wbx;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
    }
}
