using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;

public class TupleConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 2)
        {
            return ValueTuple.Create(values[0], values[1]);
        }
        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}