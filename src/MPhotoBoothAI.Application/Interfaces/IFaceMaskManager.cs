using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceMaskManager
{
    Mat GetMask(Mat targetAlignFaceAlign, Mat swapPredict);
}
