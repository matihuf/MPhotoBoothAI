using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Models;
public class UserSettings : INotifyPropertyChanged
{
    private string _cultureInfoName = string.Empty;
    public string CultureInfoName
    {
        get => _cultureInfoName;
        set
        {
            _cultureInfoName = value;
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
