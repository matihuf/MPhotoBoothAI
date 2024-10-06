using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceSwapManager
{
    Mat Swap(Mat source, Mat target);
}
