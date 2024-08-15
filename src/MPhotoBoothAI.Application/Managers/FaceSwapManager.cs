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
        using var sourceAlign = _faceAlignManager.GetAlign(source);
        using var targetAlign = _faceAlignManager.GetAlign(target);
        using var predict = _faceSwapPredictService.Predict(sourceAlign.Align, targetAlign.Align);
        using var mask = _faceMaskManager.GetMask(targetAlign.Align, predict);
        var swapped = _faceSwapService.Swap(mask, predict, targetAlign.Norm, target);
        return swapped;
    }
}
