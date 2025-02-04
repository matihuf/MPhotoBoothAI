using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;
public class BoolToOpacityLevelConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool changeOpacity && changeOpacity && parameter is double opacity)
        {
            return opacity;
        }
        return 1;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
