using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;

public class FaceAlignManager(IFaceDetectionService faceDetectionService, IFaceAlignService faceAlignService)
{
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;
    private readonly IFaceAlignService _faceAlignService = faceAlignService;

    public (FaceAlign source, FaceAlign target) GetAligns(Mat sourceFrame, Mat targetFrame)
    {
        using var sourceFace = _faceDetectionService.Detect(sourceFrame, 0.8f, 0.5f).First();
        var sourceAlignFace = _faceAlignService.Align(sourceFrame, sourceFace.Landmarks);

        using var targetFace = _faceDetectionService.Detect(targetFrame, 0.8f, 0.5f).First();
        var targetAlignFace = _faceAlignService.Align(targetFrame, targetFace.Landmarks);

        return (sourceAlignFace, targetAlignFace);
    }
}
