using Emgu.CV;
using Emgu.CV.Structure;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;
public class FaceDetectionManager(IFaceDetectionService faceDetectionService) : IFaceDetectionManager
{
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;

    public IEnumerable<FaceDetection> Detect(Mat frame, float confThreshold, float nmsThreshold)
        => _faceDetectionService.Detect(frame, confThreshold, nmsThreshold);

    public int Mark(Mat frame, float confThreshold, float nmsThreshold)
    {
        int faceIndex = 0;
        foreach (var face in _faceDetectionService.Detect(frame, confThreshold, nmsThreshold))
        {
            frame.DrawRoundedRectangle(face.Box, 20, new MCvScalar(0, 0, 255), 3);
            faceIndex++;
            face.Dispose();
        }
        return faceIndex;
    }
}
