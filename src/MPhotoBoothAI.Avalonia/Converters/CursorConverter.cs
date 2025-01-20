using Avalonia.Data.Converters;
using Avalonia.Input;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;
public class CursorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        if (value is MPhotoBoothAI.Models.Enums.Cursor cursor)
        {
            return new Cursor((StandardCursorType)cursor);
        }
        return new Cursor(StandardCursorType.Arrow);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
