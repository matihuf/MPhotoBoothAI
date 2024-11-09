using MPhotoBoothAI.Models.Entities.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Models.Entities;
public partial class FaceSwapTemplateGroupEntity : Entity, ICloneable, INotifyPropertyChanged
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NotifyPropertyChanged();
        }
    }

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

    public DateTime CreatedAt { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ICollection<FaceSwapTemplateEntity> Templates { get; set; } = [];

    public object Clone()
    {
        return MemberwiseClone();
    }
}
