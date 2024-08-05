using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Emgu.CV;

namespace MPhotoBoothAI.Avalonia.Converters;

public class MatConverter : IValueConverter
{
    public static readonly MatConverter Instance = new MatConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        if (!targetType.IsAssignableFrom(typeof(WriteableBitmap)))
            return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);

        var mat = value switch
        {
            //      MatImageMessage msg => msg.Image,
            Mat mata => mata,
            _ => null,
        };
        if (mat == null) return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
        if (parameter is WriteableBitmap wb)
        {
            try
            { // may not be good size
                mat.ToBitmapParallel(wb);
                return wb;
            }
            catch (ArgumentException e)
            {
                // ignored
                //        App.TryGetLogger<MatBitmapValueConverter>()?.LogError(e, "Error converting to bitmap");
            }
        }
        var wbx = new WriteableBitmap(new PixelSize(mat.Width, mat.Height), new Vector(96,96));
        mat.ToBitmapParallel(wbx);
        return wbx;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
    }
}
