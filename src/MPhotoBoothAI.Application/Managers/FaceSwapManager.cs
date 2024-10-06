using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.Managers;

public class FaceSwapManager(FaceAlignManager faceAlignManager, FaceMaskManager faceMaskManager, IFaceSwapPredictService faceSwapPredictService,
 IFaceSwapService faceSwapService, IFaceEnhancerService faceEnhancerService) : IFaceSwapManager
{
    private readonly FaceAlignManager _faceAlignManager = faceAlignManager;
    private readonly FaceMaskManager _faceMaskManager = faceMaskManager;
    private readonly IFaceSwapPredictService _faceSwapPredictService = faceSwapPredictService;
    private readonly IFaceSwapService _faceSwapService = faceSwapService;
    private readonly IFaceEnhancerService _faceEnhancerService = faceEnhancerService;

    public Mat Swap(Mat source, Mat target)
    {
        using var sourceAlign = _faceAlignManager.GetAlign(source);
        using var targetAlign = _faceAlignManager.GetAlign(target);
        using var predict = _faceSwapPredictService.Predict(sourceAlign.Align, targetAlign.Align);
        using var enhanced = _faceEnhancerService.Enhance(predict);

        using var resizedEnhanced = new Mat();
        CvInvoke.Resize(enhanced, resizedEnhanced, predict.Size);
        var rgb = new Mat();
        CvInvoke.CvtColor(resizedEnhanced, rgb, ColorConversion.Bgra2Rgb);

        using var mask = _faceMaskManager.GetMask(targetAlign.Align, rgb);
        var swapped = _faceSwapService.Swap(mask, rgb, targetAlign.Norm, target);
        return swapped;
    }
}
