using Emgu.CV;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceDetectionService
{
    IEnumerable<FaceDetection> Detect(Mat frame, float confThreshold, float nmsThreshold);
}
