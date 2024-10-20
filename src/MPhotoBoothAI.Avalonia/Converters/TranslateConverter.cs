using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MPhotoBoothAI.Avalonia.Converters;
public class TranslateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string displayName)
        {
            return null;
        }
        var translatedCultureName = Application.Assets.UI.ResourceManager.GetString(displayName);
        return string.IsNullOrEmpty(translatedCultureName) ? displayName : translatedCultureName;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
