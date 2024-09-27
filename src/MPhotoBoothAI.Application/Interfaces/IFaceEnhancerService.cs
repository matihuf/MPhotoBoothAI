using Emgu.CV;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceEnhancerService
{
    Mat Enhance(Mat face);
}
