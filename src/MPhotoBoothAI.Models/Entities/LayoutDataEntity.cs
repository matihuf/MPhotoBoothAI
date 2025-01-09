using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Models.Entities;
public class LayoutDataEntity : Entity
{
    public List<PhotoLayoutDataEntity> PhotoLayoutData { get; set; } = new List<PhotoLayoutDataEntity>();
    public List<OverlayImageDataEntity> OverlayImageData { get; set; } = new List<OverlayImageDataEntity>();
}
