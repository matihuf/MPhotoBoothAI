using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Models.Entities;
public class FaceSwapTemplateGroupEntity : BaseEntity, INotifyPropertyChanged
{
    public string Name { get; set; }

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            NotifyPropertyChanged();
        }
    }

    public ICollection<FaceSwapTemplateEntity> Templates { get; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
