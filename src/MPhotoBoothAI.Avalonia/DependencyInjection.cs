using System.IO;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Infrastructure.Services.Swap;

namespace MPhotoBoothAI.Avalonia;

public static class DependencyInjection
{
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        AddViewModels(services);
        AddServices(services);
        AddCamera(services);
        AddAiModels(services);
        return services;
    }

    private static void AddAiModels(IServiceCollection services)
    {
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        services.AddKeyedSingleton(Consts.AiModels.Yolov8nFace, DnnInvoke.ReadNetFromONNX($"{directory}/yolov8n-face.onnx"));
        services.AddKeyedSingleton(Consts.AiModels.ArcfaceBackbone, DnnInvoke.ReadNetFromONNX($"{directory}/arcface_backbone.onnx"));
        services.AddKeyedSingleton(Consts.AiModels.Gunet2blocks, DnnInvoke.ReadNetFromONNX($"{directory}/G_unet_2blocks.onnx"));
        services.AddKeyedSingleton(Consts.AiModels.FaceLandmarks, DnnInvoke.ReadNetFromONNX($"{directory}/face_landmarks.onnx"));
    }

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<FaceDetectionViewModel>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<ResizeImageService>();
        services.AddTransient<IFaceDetectionService, FaceDetectionService>();
        services.AddTransient<FaceSwapPredictService>();
        services.AddTransient<FaceSwapService>();
        services.AddTransient<FaceAlignService>();
        services.AddTransient<FaceLandmarksService>();
        services.AddTransient<FaceMaskService>();
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraService, CameraService>();
        services.AddSingleton((src) => new VideoCapture(0));
    }
}
