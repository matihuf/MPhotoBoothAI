using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Extensions.Options;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Models.Configurations;

namespace MPhotoBoothAI.Application.Managers;
public class FaceDetectionManager(IFaceDetectionService faceDetectionService, IOptions<FaceDetectionConfiguration> faceDetectionConfiguration) : IFaceDetectionManager
{
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;
    private readonly IOptions<FaceDetectionConfiguration> _faceDetectionConfiguration = faceDetectionConfiguration;

    public IEnumerable<FaceDetection> Detect(Mat frame)
        => _faceDetectionService.Detect(frame, _faceDetectionConfiguration.Value.ConfThreshold, _faceDetectionConfiguration.Value.NmsThreshold);

    public int Mark(Mat frame)
    {
        int faceIndex = 0;
        foreach (var face in _faceDetectionService.Detect(frame, _faceDetectionConfiguration.Value.ConfThreshold, _faceDetectionConfiguration.Value.NmsThreshold))
        {
            frame.DrawRoundedRectangle(face.Box, 20, new MCvScalar(0, 0, 255), 3);
            faceIndex++;
            face.Dispose();
        }
        return faceIndex;
    }
}
