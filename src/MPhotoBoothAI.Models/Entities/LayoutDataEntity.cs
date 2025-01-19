using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Models.Entities;
public class LayoutDataEntity : Entity
{
    public List<PhotoLayoutDataEntity> PhotoLayoutData { get; set; } = [];
    public List<OverlayImageDataEntity> OverlayImageData { get; set; } = [];
}
