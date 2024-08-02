using Emgu.CV;

namespace MPhotoBoothAI.Application;

public interface IYoloFaceService
{
    void Run(Mat frame, float confThreshold, float nmsThreshold);
}
