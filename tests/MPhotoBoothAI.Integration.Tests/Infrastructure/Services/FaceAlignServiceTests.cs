using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceAlignServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceAlignService _faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
    private readonly IFaceDetectionService _faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
   
    [Fact]
    public void Align_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanAlign.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var sourceFace = _faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        //act
        using var result = _faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);
        //assert
        string tmpFile = $"{nameof(FaceAlignServiceTests)}womanAlignTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result.Align);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
