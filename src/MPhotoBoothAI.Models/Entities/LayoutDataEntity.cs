using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Models.Entities;
public class LayoutDataEntity : Entity
{
    public ICollection<PhotoLayoutDataEntity> PhotoLayoutData { get; set; } = [];
    public ICollection<OverlayImageDataEntity> OverlayImageData { get; set; } = [];
}
