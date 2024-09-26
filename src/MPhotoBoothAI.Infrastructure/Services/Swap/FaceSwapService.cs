using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services.Swap;

public class FaceSwapService : IFaceSwapService, IDisposable
{
    private readonly ScalarArray _oneScalarArray = new(1.0);

    public Mat Swap(Mat mask, Mat swapPredict, Mat targetAlignFaceNorm, Mat target)
    {
        using var mat_rev = new Mat();
        CvInvoke.InvertAffineTransform(targetAlignFaceNorm, mat_rev);
        using var swap_t = new Mat();
        CvInvoke.WarpAffine(swapPredict, swap_t, mat_rev, target.Size, borderMode: BorderType.Replicate);

        using var premask_t = new Mat();
        CvInvoke.WarpAffine(mask, premask_t, mat_rev, target.Size);
        using var mask_t = new Mat(premask_t.Size, DepthType.Cv32F, 1);
        using var channels = new VectorOfMat(premask_t, premask_t, premask_t);
        CvInvoke.Merge(channels, mask_t);

        using var oneMinusWarpAffine = new Mat();
        CvInvoke.Subtract(_oneScalarArray, mask_t, oneMinusWarpAffine);

        using var partOne = new Mat();
        using var swap_t64 = new Mat();
        swap_t.ConvertTo(swap_t64, DepthType.Cv32F);
        CvInvoke.Multiply(mask_t, swap_t64, partOne);
        using var partOneBgr = new Mat();
        CvInvoke.CvtColor(partOne, partOneBgr, ColorConversion.Rgb2Bgr);

        using var partTwo = new Mat();
        using var final_64 = new Mat();
        target.ConvertTo(final_64, DepthType.Cv32F);
        CvInvoke.Multiply(oneMinusWarpAffine, final_64, partTwo);

        using var sum = new Mat();
        CvInvoke.Add(partOneBgr, partTwo, sum);
        var result = new Mat();
        sum.ConvertTo(result, DepthType.Cv8U);
        return result;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _oneScalarArray.Dispose();
        }
    }
}