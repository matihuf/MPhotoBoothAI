using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceEnhancerServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void Enhance_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/womanFaceEnhanced.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/swappedLow.dat");

        var faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        var faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
        using var sourceAlignFace = faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);
        var faceEnhancerService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceEnhancerService>();
        //act
        using var enhanced = faceEnhancerService.Enhance(sourceAlignFace.Align);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, enhanced));
    }
}
