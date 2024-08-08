using Emgu.CV;
using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Infrastructure.Services;

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
        services.AddTransient<IFaceDetectionService, FaceDetectionService>();
        services.AddSingleton((src) =>  DnnInvoke.ReadNetFromONNX("/workspaces/MPhotoBoothAI/src/MPhotoBoothAI.Avalonia/bin/Debug/net8.0/yolov8n-face.onnx"));
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraService, CameraService>();
        services.AddSingleton((src) => new VideoCapture(0));
    }
}
