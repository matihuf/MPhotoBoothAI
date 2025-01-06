using Emgu.CV;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceDetectionManager
{
    IEnumerable<FaceDetection> Detect(Mat frame, float confThreshold, float nmsThreshold);

    /// <summary>
    /// Mark faces with rectangle
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="confThreshold"></param>
    /// <param name="nmsThreshold"></param>
    /// <returns>Number of founded faces</returns>
    int Mark(Mat frame, float confThreshold, float nmsThreshold);
}
