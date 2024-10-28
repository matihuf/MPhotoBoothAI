namespace MPhotoBoothAI.Models.Entities;
public class FaceSwapTemplateEntity : BaseEntity
{
    public int Faces { get; set; }
    public string FileName { get; set; }
    public int FaceSwapTemplateGroupId { get; set; }
    public FaceSwapTemplateGroupEntity FaceSwapTemplateGroup { get; set; } = null!;
}
