using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.Managers;

public class FaceSwapManager(IFaceAlignManager faceAlignManager, FaceMaskManager faceMaskManager, IFaceSwapPredictService faceSwapPredictService,
 IFaceSwapService faceSwapService, IFaceEnhancerService faceEnhancerService) : IFaceSwapManager
{
    private readonly IFaceAlignManager _faceAlignManager = faceAlignManager;
    private readonly FaceMaskManager _faceMaskManager = faceMaskManager;
    private readonly IFaceSwapPredictService _faceSwapPredictService = faceSwapPredictService;
    private readonly IFaceSwapService _faceSwapService = faceSwapService;
    private readonly IFaceEnhancerService _faceEnhancerService = faceEnhancerService;

    public Mat Swap(Mat source, Mat target)
    {
        var swapped = target.Clone();
        var sourceAligns = _faceAlignManager.GetAligns(source);
        var targetAligns = _faceAlignManager.GetAligns(target);
        foreach (var targetAlign in targetAligns)
        {
            using var sourceAlign = sourceAligns.FirstOrDefault(x => x.Gender == targetAlign.Gender);
            if (sourceAlign != null)
            {
                using var predict = _faceSwapPredictService.Predict(sourceAlign.Align, targetAlign.Align);
                using var enhanced = _faceEnhancerService.Enhance(predict);
                using var mask = _faceMaskManager.GetMask(targetAlign.Align, enhanced);
                swapped = _faceSwapService.Swap(mask, enhanced, targetAlign.Norm, swapped);
            }
            targetAlign.Dispose();
        }
        return swapped;
    }
}
