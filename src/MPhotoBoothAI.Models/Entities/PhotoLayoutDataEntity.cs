using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Application.Models;
public class PhotoLayoutDataEntity : Entity
{
    public double Top { get; set; }
    public double Left { get; set; }
    public double Angle { get; set; }
    public double Scale { get; set; }
}
