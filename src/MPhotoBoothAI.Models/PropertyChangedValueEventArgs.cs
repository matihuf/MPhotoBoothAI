using System.ComponentModel;

namespace MPhotoBoothAI.Models;
public class PropertyChangedValueEventArgs(string? propertyName, object newValue) : PropertyChangedEventArgs(propertyName)
{
    public object? NewValue { get; } = newValue;
}
