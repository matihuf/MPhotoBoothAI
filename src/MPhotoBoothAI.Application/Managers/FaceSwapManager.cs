using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.Managers;

public class FaceSwapManager(FaceAlignManager faceAlignManager, FaceMaskManager faceMaskManager, IFaceSwapPredictService faceSwapPredictService, IFaceSwapService faceSwapService)
{
    private readonly FaceAlignManager _faceAlignManager = faceAlignManager;
    private readonly FaceMaskManager _faceMaskManager = faceMaskManager;
    private readonly IFaceSwapPredictService _faceSwapPredictService = faceSwapPredictService;
    private readonly IFaceSwapService _faceSwapService = faceSwapService;

    public Mat Swap(Mat source, Mat target)
    {
        var aligns = _faceAlignManager.GetAligns(source, target);
        try
        {
            using var predict = _faceSwapPredictService.Predict(aligns.source.Align, aligns.target.Align);
            using var mask = _faceMaskManager.GetMask(aligns.target.Align, predict);
            var swapped = _faceSwapService.Swap(mask, predict, aligns.target.Norm, target);
            return swapped;
        }
        finally
        {
            aligns.target.Dispose();
            aligns.source.Dispose();
        }
    }
}
