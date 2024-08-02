using Emgu.CV;
using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Infrastructure;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia;

public static class DependencyInjection
{
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        AddViewModels(services);
        AddServices(services);
        AddCamera(services);
        return services;
    }

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<FaceDetectionViewModel>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<ResizeImageService>();
        services.AddTransient<IYoloFaceService, YoloFaceService>();
        services.AddSingleton((src) =>  DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx"));
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraService, CameraService>();
        services.AddSingleton((src) => new VideoCapture(0));
    }
}
