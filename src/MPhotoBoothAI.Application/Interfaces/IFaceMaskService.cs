using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceMaskService
{
    Mat GetMask(Mat frame, float[,] frameLandmarks, float[,] targetLandmarks);
}
