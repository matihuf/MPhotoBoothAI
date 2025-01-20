using Emgu.CV;
using MPhotoBoothAI.Models;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IResizeImageService
{
    Mat GetThumbnail(Mat frame, float percentage);
    ResizedImage Resize(Mat frame, int inputHeight = 640, int inputWidth = 640, bool keepRatio = true);
}
