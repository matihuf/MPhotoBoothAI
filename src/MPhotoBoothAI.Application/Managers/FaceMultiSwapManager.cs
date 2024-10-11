using Emgu.CV;
using Microsoft.Extensions.Logging;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Managers;

public class FaceMultiSwapManager(IFaceAlignManager faceAlignManager, IFaceSwapManager faceSwapManager, ILogger<FaceMultiSwapManager> logger) : IFaceMultiSwapManager
{
    private readonly IFaceAlignManager _faceAlignManager = faceAlignManager;
    private readonly IFaceSwapManager _faceSwapManager = faceSwapManager;
    private readonly ILogger<FaceMultiSwapManager> _logger = logger;

    public Mat Swap(Mat source, Mat target)
    {
        var swapped = target.Clone();
        var targetAligns = _faceAlignManager.GetAligns(target).ToList();
        foreach (var sourceAlign in _faceAlignManager.GetAligns(source))
        {
            try
            {
                using var targetAlign = targetAligns.FirstOrDefault(x => x.Gender == sourceAlign.Gender) ?? targetAligns.FirstOrDefault();
                if (targetAlign != null)
                {
                    targetAligns.Remove(targetAlign);
                    swapped = _faceSwapManager.Swap(sourceAlign, targetAlign, swapped);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Multi swap failed");
            }
            finally
            {
                sourceAlign.Dispose();
            }
        }
        ClearNotUsedTargets(targetAligns);
        return swapped;
    }

    private static void ClearNotUsedTargets(IList<FaceAlignDetails> targetAligns)
    {
        foreach (var targetAlign in targetAligns)
        {
            targetAlign.Dispose();
        }
    }
}
