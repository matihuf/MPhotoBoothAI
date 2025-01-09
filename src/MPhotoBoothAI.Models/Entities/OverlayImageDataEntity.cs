using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Models.Entities
{
    public class OverlayImageDataEntity : Entity
    {
        public string Path { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Angle { get; set; }
        public double Scale { get; set; }
        public LayoutDataEntity LayoutData { get; set; }
        public int LayoutDataEntityId { get; set; }

    }
}