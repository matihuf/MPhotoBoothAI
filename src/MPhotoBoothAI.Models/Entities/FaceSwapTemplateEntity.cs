using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Models.Entities;
public class FaceSwapTemplateEntity : Entity
{
    public int Faces { get; set; }
    public int FaceSwapTemplateGroupId { get; set; }
    public DateTime CreatedAt { get; set; }
    public FaceSwapTemplateGroupEntity FaceSwapTemplateGroup { get; set; } = null!;
}
