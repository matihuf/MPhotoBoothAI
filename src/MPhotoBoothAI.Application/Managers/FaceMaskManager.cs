using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.Managers;

public class FaceMaskManager(IFaceLandmarksService faceLandmarksService, IFaceMaskService faceMaskService) : IFaceMaskManager
{
    private readonly IFaceLandmarksService _faceLandmarksService = faceLandmarksService;
    private readonly IFaceMaskService _faceMaskService = faceMaskService;

    public Mat GetMask(Mat targetAlignFaceAlign, Mat swapPredict)
    {
        var predictLandmarks = _faceLandmarksService.GetLandmarks(swapPredict);
        var targetLandmarks = _faceLandmarksService.GetLandmarks(targetAlignFaceAlign);
        var mask = _faceMaskService.GetMask(targetAlignFaceAlign, predictLandmarks, targetLandmarks);
        return mask;
    }
}
