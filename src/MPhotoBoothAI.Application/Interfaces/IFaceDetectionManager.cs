using Emgu.CV;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceDetectionManager
{
    IEnumerable<FaceDetection> Detect(Mat frame);

    /// <summary>
    /// Mark faces with rectangle
    /// </summary>
    /// <param name="frame"></param>
    /// <returns>Number of founded faces</returns>
    int Mark(Mat frame);
}
