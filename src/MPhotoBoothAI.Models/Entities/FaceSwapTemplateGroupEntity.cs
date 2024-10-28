namespace MPhotoBoothAI.Models.Entities;
public class FaceSwapTemplateGroupEntity : BaseEntity
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public ICollection<FaceSwapTemplateEntity> Templates { get; } = [];
}
