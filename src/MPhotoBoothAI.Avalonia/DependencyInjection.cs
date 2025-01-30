using EDSDK.NET;
using Emgu.CV.Dnn;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML.OnnxRuntime;
using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;
using MPhotoBoothAI.Application.Managers.FaceSwapTemplates;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
using MPhotoBoothAI.Avalonia.Navigation;
using MPhotoBoothAI.Avalonia.Services;
using MPhotoBoothAI.Infrastructure.CameraDevices;
using MPhotoBoothAI.Infrastructure.Persistence;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Infrastructure.Services.Swap;
using Serilog;
using System.IO;
using System.Reflection;

namespace MPhotoBoothAI.Avalonia;

public static class DependencyInjection
{
    public static IServiceCollection Configure(this IServiceCollection services, IConfiguration configuration, bool addAiModels = true)
    {
        AddViewModels(services);
        AddServices(services);
        AddCamera(services);
        if (addAiModels)
        {
            AddAiModels(services);
        }
        AddManagers(services);
        AddNavigation(services);
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        AddDatabase(services);
        return services;
    }

    private static void AddDatabase(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(o => o.UseSqlite(), ServiceLifetime.Transient);
        services.AddTransient<IDatabaseContext, DatabaseContext>(s => s.GetRequiredService<DatabaseContext>());
    }

    private static void AddNavigation(IServiceCollection services)
    {
        services.AddSingleton<INavigationService<ViewModelBase>>(s => new HistoryRouter<ViewModelBase>(t => (ViewModelBase)s.GetRequiredService(t)));
    }

    private static void AddManagers(IServiceCollection services)
    {
        services.AddTransient<IFaceAlignManager, FaceAlignManager>();
        services.AddTransient<IFaceMaskManager, FaceMaskManager>();
        services.AddTransient<IFaceSwapManager, FaceSwapManager>();
        services.AddTransient<IFaceMultiSwapManager, FaceMultiSwapManager>();
        services.AddSingleton<ICameraManager, CameraManager>();
        services.AddTransient<IAddFaceSwapTemplateManager, AddFaceSwapTemplateManager>();
        services.AddTransient<IFaceSwapTemplateFileManager, FaceSwapTemplateFileManager>();
        services.AddTransient<IFaceDetectionManager, FaceDetectionManager>();
    }

    private static void AddAiModels(IServiceCollection services)
    {
        services.AddKeyedSingleton(Consts.AiModels.Yolov8nFace, delegate { return new LazyDisposal<Net>(() => GetDnnModel(Consts.AiModels.Yolov8nFace)); });
        services.AddKeyedSingleton(Consts.AiModels.ArcfaceBackbone, delegate { return new LazyDisposal<Net>(() => GetDnnModel(Consts.AiModels.ArcfaceBackbone)); });
        services.AddKeyedSingleton(Consts.AiModels.Gunet2blocks, delegate { return new LazyDisposal<Net>(() => GetDnnModel(Consts.AiModels.Gunet2blocks)); });
        services.AddKeyedSingleton(Consts.AiModels.FaceLandmarks, delegate { return new LazyDisposal<Net>(() => GetDnnModel(Consts.AiModels.FaceLandmarks)); });
        services.AddKeyedSingleton(Consts.AiModels.VggGender, delegate { return new LazyDisposal<Net>(() => GetDnnModel(Consts.AiModels.VggGender)); });
        services.AddKeyedSingleton(Consts.AiModels.Gfpgan, delegate { return new LazyDisposal<InferenceSession>(() => new InferenceSession(GetModelPath(Consts.AiModels.Gfpgan))); });
    }

    private static Net GetDnnModel(string name) => DnnInvoke.ReadNetFromONNX(GetModelPath(name));

    private static string GetModelPath(string name) => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "models", $"{name}.onnx");

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<FaceDetectionViewModel>();
        services.AddTransient<LanguageViewModel>();
        services.AddTransient<CameraSettingsViewModel>();
        services.AddTransient<FaceSwapTemplatesViewModel>();
        services.AddTransient<DesignPrintTemplateViewModel>();
        services.AddTransient<FaceSwapGroupTemplatesViewModel>();
        services.AddTransient<AddFaceSwapTemplateViewModel>();
        services.AddTransient<PreviewFaceSwapTemplateViewModel>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<IResizeImageService, ResizeImageService>();
        services.AddTransient<IFaceDetectionService, FaceDetectionService>();
        services.AddTransient<IFaceSwapPredictService, FaceSwapPredictService>();
        services.AddTransient<IFaceSwapService, FaceSwapService>();
        services.AddTransient<IFaceAlignService, FaceAlignService>();
        services.AddTransient<IFaceLandmarksService, FaceLandmarksService>();
        services.AddTransient<IFaceMaskService, FaceMaskService>();
        services.AddTransient<IFilePickerService, FilePickerService>();
        services.AddTransient<IFaceEnhancerService, FaceEnhancerService>();
        services.AddTransient<IFaceGenderService, FaceGenderService>();
        services.AddTransient<IAppRestarterService, AppRestarterService>();
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        services.AddTransient<IDiskInfoService, DiskInfoService>();
        services.AddTransient<SDKHandler>();
        services.AddTransient<IMessageBoxService, MessageBoxService>();
        services.AddTransient<IFilesManager, FilesManager>();
        services.AddTransient<IImageManager, ImagesManager>();
        services.AddTransient<IWindowService, WindowService>();
    }

    private static void AddCamera(IServiceCollection services)
    {
        services.AddSingleton<ICameraDevice, WebCameraDevice>();
        services.AddSingleton<ICameraDevice, CanonCameraDevice>();
    }
}
