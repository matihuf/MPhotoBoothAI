using Avalonia.Data.Converters;
using MPhotoBoothAI.Application.Models;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;
public class FormatTypeToTranslatedNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int formatIndex)
        {
            var lowerCaseName = ((FormatTypes)formatIndex).ToString().ToLowerInvariant();
            var translatedName = Application.Assets.UI.ResourceManager.GetString(lowerCaseName);
            return translatedName;
        }
        return "Format Not Found";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
