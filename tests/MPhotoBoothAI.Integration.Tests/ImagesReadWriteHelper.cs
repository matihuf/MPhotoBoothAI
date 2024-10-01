using Emgu.CV.CvEnum;

namespace MPhotoBoothAI.Integration.Tests
{
    public class ImagesReadWriteHelper
    {
        private readonly Dictionary<ImwriteFlags, int> _imageSaveOptions = [];

        public KeyValuePair<ImwriteFlags, int>[] ImageSaveOptions => _imageSaveOptions.ToArray();

        public ImagesReadWriteHelper()
        {
            _imageSaveOptions.Add(ImwriteFlags.PngCompression, 0);
        }
    }
}
