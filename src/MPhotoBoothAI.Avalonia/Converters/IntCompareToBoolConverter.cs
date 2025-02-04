using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;
public class IntCompareToBoolConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is int template && values[1] is int reference)
        {
            return template == reference;
        }
        return false;
    }
}
