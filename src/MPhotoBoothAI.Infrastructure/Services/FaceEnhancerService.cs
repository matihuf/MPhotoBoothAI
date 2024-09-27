using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML.OnnxRuntime;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services;

public class FaceEnhancerService([FromKeyedServices(Consts.AiModels.Gfpgan)] InferenceSession gfpgan) : IFaceEnhancerService
{
    private readonly InferenceSession _gfpgan = gfpgan;

    public Mat Enhance(Mat frame)
    {
        return frame;
    }
}