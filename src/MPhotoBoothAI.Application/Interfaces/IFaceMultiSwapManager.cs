using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceMultiSwapManager
{
    Mat Swap(Mat source, Mat target);
}
