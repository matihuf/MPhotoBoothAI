using Avalonia.Data.Converters;
using Material.Icons;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;

public class IconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        if (Enum.TryParse(typeof(MaterialIconKind), value.ToString() ?? string.Empty, true, out var iconKind))
        {
            return iconKind;
        }
        return MaterialIconKind.Home;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}