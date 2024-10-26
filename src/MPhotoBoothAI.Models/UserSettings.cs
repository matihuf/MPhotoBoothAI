using MPhotoBoothAI.Models.Base;

namespace MPhotoBoothAI.Models;
public class UserSettings : BaseSettings
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
}
