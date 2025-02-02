using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using System.Drawing;

namespace MPhotoBoothAI.Application.Managers;
public class ImagesManager : IImageManager
{
    public Size? GetImageSizeFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        try
        {
            using var image = CvInvoke.Imread(path);
            return image.Size;
        }
        catch
        {
            return null;
        }
    }
}