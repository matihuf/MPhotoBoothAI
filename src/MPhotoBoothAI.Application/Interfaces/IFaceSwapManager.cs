using Emgu.CV;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceSwapManager
{
    Mat Swap(FaceAlign sourceAlign, FaceAlign targetAlign, Mat target);
}
