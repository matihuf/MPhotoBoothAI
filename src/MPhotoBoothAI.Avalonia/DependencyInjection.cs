using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML.OnnxRuntime;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Navigation;
using MPhotoBoothAI.Avalonia.Services;
using MPhotoBoothAI.Infrastructure.CameraDevices;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Infrastructure.Services.Swap;
using System.IO;
using System.Reflection;
using Serilog;

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
        AddNavigation(services);
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        return services;
    }

    private static void AddNavigation(IServiceCollection services)
    {
        services.AddSingleton<INavigationService<ViewModelBase>>(s => new HistoryRouter<ViewModelBase>(t => (ViewModelBase)s.GetRequiredService(t)));
    }

    private static void AddManagers(IServiceCollection services)
    {
        services.AddTransient<IFaceAlignManager, FaceAlignManager>();
        services.AddTransient<FaceMaskManager>();
        services.AddTransient<IFaceSwapManager, FaceSwapManager>();
    }

    private static void AddAiModels(IServiceCollection services)
    {
        services.AddKeyedSingleton(Consts.AiModels.Yolov8nFace, GetDnnModel(Consts.AiModels.Yolov8nFace));
        services.AddKeyedSingleton(Consts.AiModels.ArcfaceBackbone, GetDnnModel(Consts.AiModels.ArcfaceBackbone));
        services.AddKeyedSingleton(Consts.AiModels.Gunet2blocks, GetDnnModel(Consts.AiModels.Gunet2blocks));
        services.AddKeyedSingleton(Consts.AiModels.FaceLandmarks, GetDnnModel( Consts.AiModels.FaceLandmarks));
        services.AddKeyedSingleton(Consts.AiModels.VggGender, GetDnnModel( Consts.AiModels.VggGender));
        services.AddKeyedSingleton(Consts.AiModels.Gfpgan, new InferenceSession(GetModelPath(Consts.AiModels.Gfpgan)));
    }

    private static Net GetDnnModel(string name) => DnnInvoke.ReadNetFromONNX(GetModelPath(name));
    private static string GetModelPath(string name) => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "models", $"{name}.onnx");

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddTransient<HomeViewModel>();
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
        services.AddTransient<IFaceEnhancerService, FaceEnhancerService>();
        services.AddTransient<IFaceGenderService, FaceGenderService>();
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraDevice, WebCameraDevice>();
    }
}
