namespace MPhotoBoothAI.Models.Camera
{
    public class CameraSetting
    {
        public string? Current { get; set; }

        public IEnumerable<string>? AvailableValues { get; set; }
    }
}
