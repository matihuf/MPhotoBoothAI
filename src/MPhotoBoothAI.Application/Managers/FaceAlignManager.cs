using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;

public class FaceAlignManager(IFaceDetectionService faceDetectionService, IFaceAlignService faceAlignService, IFaceGenderService faceGenderService) : IFaceAlignManager
{
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;
    private readonly IFaceAlignService _faceAlignService = faceAlignService;
    private readonly IFaceGenderService _faceGenderService = faceGenderService;

    public FaceAlign? GetAlign(Mat frame)
    {
        using var face = _faceDetectionService.Detect(frame, 0.8f, 0.5f).FirstOrDefault();
        if (face == null)
        {
            return null;
        }
        return _faceAlignService.Align(frame, face.Landmarks);
    }

    public IEnumerable<FaceAlignDetails> GetAligns(Mat frame)
    {
        foreach (var face in _faceDetectionService.Detect(frame, 0.8f, 0.5f))
        {
            var faceAlign = _faceAlignService.Align(frame, face.Landmarks);
            face.Dispose();
            var gender = _faceGenderService.Get(faceAlign.Align);
            yield return new FaceAlignDetails(faceAlign.Norm, faceAlign.Align, gender);
        }
    }
}
