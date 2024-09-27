using Emgu.CV;
using Emgu.CV.Util;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceAlignService
{
    FaceAlign Align(Mat frame, VectorOfPointF landmarks, int cropSize = 224);
}
