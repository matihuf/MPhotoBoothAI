using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML.OnnxRuntime;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;
using MPhotoBoothAI.Application.Navigation;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Services;
using MPhotoBoothAI.Infrastructure.CameraDevices;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Infrastructure.Services.Swap;
using System.IO;
using System.Reflection;

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
        return services;
    }

    private static void AddNavigation(IServiceCollection services)
    {
        services.AddSingleton<IHistoryRouter<ViewModelBase>>(s => new HistoryRouter<ViewModelBase>(t => (ViewModelBase)s.GetRequiredService(t)));
        services.AddSingleton<INavigationService, NavigationService>();
    }

    private static void AddManagers(IServiceCollection services)
    {
        services.AddTransient<FaceAlignManager>();
        services.AddTransient<FaceMaskManager>();
        services.AddTransient<IFaceSwapManager, FaceSwapManager>();
    }

    private static void AddAiModels(IServiceCollection services)
    {
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        services.AddKeyedSingleton(Consts.AiModels.Yolov8nFace, GetDnnModel(directory, Consts.AiModels.Yolov8nFace));
        services.AddKeyedSingleton(Consts.AiModels.ArcfaceBackbone, GetDnnModel(directory, Consts.AiModels.ArcfaceBackbone));
        services.AddKeyedSingleton(Consts.AiModels.Gunet2blocks, GetDnnModel(directory, Consts.AiModels.Gunet2blocks));
        services.AddKeyedSingleton(Consts.AiModels.FaceLandmarks, GetDnnModel(directory, Consts.AiModels.FaceLandmarks));
        services.AddKeyedSingleton(Consts.AiModels.Gfpgan, new InferenceSession($"{directory}/{Consts.AiModels.Gfpgan}.onnx"));
    }

    private static Net GetDnnModel(string directory, string name) => DnnInvoke.ReadNetFromONNX($"{directory}/{name}.onnx");

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
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraDevice, WebCameraDevice>();
    }
}
