using System.Drawing;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IImageManager
{
    Size? GetImageSizeFromFile(string path);
}
