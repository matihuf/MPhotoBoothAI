namespace MPhotoBoothAI.Models.Entities
{
    public class CameraSettingsEntity : BaseEntity
    {
        public string Iso { get; set; }

        public string Aperture { get; set; }

        public string ShutterSpeed { get; set; }

        public string WhiteBalance { get; set; }
    }
}
