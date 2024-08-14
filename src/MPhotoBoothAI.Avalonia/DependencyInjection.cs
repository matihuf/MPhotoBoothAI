using System.IO;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Services;
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
        AddManagers(services);
        return services;
    }

    private static void AddManagers(IServiceCollection services)
    {
        services.AddTransient<FaceAlignManager>();
        services.AddTransient<FaceMaskManager>();
        services.AddTransient<FaceSwapManager>();
    }

    private static void AddAiModels(IServiceCollection services)
    {
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        services.AddKeyedSingleton(Consts.AiModels.Yolov8nFace, GetModel(directory, Consts.AiModels.Yolov8nFace));
        services.AddKeyedSingleton(Consts.AiModels.ArcfaceBackbone, GetModel(directory, Consts.AiModels.ArcfaceBackbone));
        services.AddKeyedSingleton(Consts.AiModels.Gunet2blocks, GetModel(directory, Consts.AiModels.Gunet2blocks));
        services.AddKeyedSingleton(Consts.AiModels.FaceLandmarks, GetModel(directory, Consts.AiModels.FaceLandmarks));
    }

    private static Net GetModel(string directory, string name) => DnnInvoke.ReadNetFromONNX($"{directory}/{name}.onnx");

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<FaceDetectionViewModel>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<ResizeImageService>();
        services.AddTransient<IFaceDetectionService, FaceDetectionService>();
        services.AddTransient<IFaceSwapPredictService, FaceSwapPredictService>();
        services.AddTransient<IFaceSwapService, FaceSwapService>();
        services.AddTransient<IFaceAlignService, FaceAlignService>();
        services.AddTransient<IFaceLandmarksService, FaceLandmarksService>();
        services.AddTransient<IFaceMaskService, FaceMaskService>();
        services.AddTransient<IFilePickerService, FilePickerService>();
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraService, CameraService>();
        services.AddSingleton((src) => new VideoCapture(0));
    }
}
