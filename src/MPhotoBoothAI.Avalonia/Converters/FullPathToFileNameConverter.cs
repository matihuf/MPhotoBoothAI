using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.IO;

namespace MPhotoBoothAI.Avalonia.Converters;
public class FullPathToFileNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string path && !String.IsNullOrEmpty(path))
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
