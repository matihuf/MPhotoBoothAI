using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceSwapPredictService
{
    Mat Predict(Mat sourceFace, Mat targetFace);
}
