using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceLandmarksService : IDisposable
{
    float[,] GetLandmarks(Mat frame);
}
