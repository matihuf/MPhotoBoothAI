using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;

public class FaceSwapManager(IFaceMaskManager faceMaskManager, IFaceSwapPredictService faceSwapPredictService,
    IFaceSwapService faceSwapService, IFaceEnhancerService faceEnhancerService) : IFaceSwapManager
{
    private readonly IFaceMaskManager _faceMaskManager = faceMaskManager;
    private readonly IFaceSwapPredictService _faceSwapPredictService = faceSwapPredictService;
    private readonly IFaceSwapService _faceSwapService = faceSwapService;
    private readonly IFaceEnhancerService _faceEnhancerService = faceEnhancerService;

    public Mat Swap(FaceAlign sourceAlign, FaceAlign targetAlign, Mat target)
    {
        using var predict = _faceSwapPredictService.Predict(sourceAlign.Align, targetAlign.Align);
        using var enhanced = _faceEnhancerService.Enhance(predict);
        using var mask = _faceMaskManager.GetMask(targetAlign.Align, enhanced);
        return _faceSwapService.Swap(mask, enhanced, targetAlign.Norm, target);
    }
}
