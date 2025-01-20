using Emgu.CV;
using Emgu.CV.Structure;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;
public class FaceDetectionManager(IFaceDetectionService faceDetectionService) : IFaceDetectionManager
{
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;

    private readonly float _confThreshold = 0.8f;
    private readonly float _nmsThreshold = 0.5f;
    private readonly int _markRadius = 20;
    private readonly int _markThickness = 3;
    private readonly MCvScalar _markColor = new(0, 0, 255);

    public IEnumerable<FaceDetection> Detect(Mat frame)
        => _faceDetectionService.Detect(frame, _confThreshold, _nmsThreshold);

    public int Mark(Mat frame)
    {
        int faceIndex = 0;
        foreach (var face in _faceDetectionService.Detect(frame, _confThreshold, _nmsThreshold))
        {
            frame.DrawRoundedRectangle(face.Box, _markRadius, _markColor, _markThickness);
            faceIndex++;
            face.Dispose();
        }
        return faceIndex;
    }
}
