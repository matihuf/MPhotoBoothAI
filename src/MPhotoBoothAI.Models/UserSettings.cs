using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Models;
public class UserSettings
{
    private string _cultureInfoName = string.Empty;
    public string CultureInfoName
    {
        get => _cultureInfoName;
        set
        {
            _cultureInfoName = value;
            NotifyPropertyChanged(value);
        }
    }

    public delegate void PropertyChangedValueEventHandler(object? sender, PropertyChangedValueEventArgs e);
    public event PropertyChangedValueEventHandler? PropertyChanged;

    private void NotifyPropertyChanged(object newValue, [CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedValueEventArgs(propertyName, newValue));
}
