using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Application.Models;
public class LayoutFormatEntity : Entity
{
    public double FormatWidth { get; set; }

    public double FormatHeight { get; set; }

    public double FormatRatio { get; set; }

    public string SizeName { get; set; } = "";
}
