using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;

public class FaceAlignManager(IFaceDetectionService faceDetectionService, IFaceAlignService faceAlignService) : IFaceAlignManager
{
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;
    private readonly IFaceAlignService _faceAlignService = faceAlignService;

    public FaceAlign GetAlign(Mat frame)
    {
        using var face = _faceDetectionService.Detect(frame, 0.8f, 0.5f).FirstOrDefault();
        if (face == null)
        {
            return null;
        }
        var alignFace = _faceAlignService.Align(frame, face.Landmarks);
        return alignFace;
    }
}
