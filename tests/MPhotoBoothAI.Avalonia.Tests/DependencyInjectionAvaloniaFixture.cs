using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Avalonia.Tests;
public class DependencyInjectionAvaloniaFixture : DependencyInjectionFixture
{
    public override string Configuration { get; set; } = "avalonia";

    public override void ReplaceService(IServiceCollection services)
    {
        base.ReplaceService(services);
        ReplaceAiServices(services);
    }

    private static void ReplaceAiServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient(s => new Mock<IFaceDetectionService>().Object));
        services.Replace(ServiceDescriptor.Transient(s => new Mock<IFaceSwapPredictService>().Object));
        services.Replace(ServiceDescriptor.Transient(s => new Mock<IFaceLandmarksService>().Object));
        services.Replace(ServiceDescriptor.Transient(s => new Mock<IFaceEnhancerService>().Object));
        services.Replace(ServiceDescriptor.Transient(s => new Mock<IFaceGenderService>().Object));
    }
}
