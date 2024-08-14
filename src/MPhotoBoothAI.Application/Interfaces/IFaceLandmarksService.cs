using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceLandmarksService
{
    float[,] GetLandmarks(Mat frame);
}
