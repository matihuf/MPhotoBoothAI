using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceEnhancerServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceEnhancerService _faceEnhancerService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceEnhancerService>();
    private readonly IFaceDetectionService _faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
    private readonly IFaceAlignService _faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();

    [Fact]
    public void Enhance_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanFaceEnhanced.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/swappedLow.jpg");
        using var sourceFace = _faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        using var sourceAlignFace = _faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);
        //act
        using var enhanced = _faceEnhancerService.Enhance(sourceAlignFace.Align);
        //assert
        string tmpFile = $"{nameof(FaceEnhancerServiceTests)}enhanced.jpg";
        CvInvoke.Imwrite(tmpFile, enhanced);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
