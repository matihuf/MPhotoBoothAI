using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceSwapService
{
    Mat Swap(Mat mask, Mat swapPredict, Mat targetAlignFaceNorm, Mat target);
}
