using CommunityToolkit.Mvvm.ComponentModel;

namespace MPhotoBoothAI.Models.Entities;
public partial class FaceSwapTemplateGroupEntity : BaseEntity, ICloneable
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private bool _isEnabled;

    public ICollection<FaceSwapTemplateEntity> Templates { get; } = [];

    public object Clone()
    {
        return new FaceSwapTemplateGroupEntity
        {
            Name = Name,
            IsEnabled = IsEnabled
        };
    }
}
