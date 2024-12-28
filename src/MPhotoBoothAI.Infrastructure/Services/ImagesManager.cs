using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using System.Drawing;

namespace MPhotoBoothAI.Infrastructure.Services;
public class ImagesManager : IImageManager
{
    public Size? GetImageSizeFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        var image = CvInvoke.Imread(path);
        return image.Size;
    }
}
