using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Application.Models;
public class LayoutImageEntity : Entity
{
    public int Width { get; set; }
    public int Heigth { get; set; }
    public double Angle { get; set; }
    public double Scale { get; set; }
    public int Top { get; set; }
    public int Left { get; set; }
}
