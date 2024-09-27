using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceAlignServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceAlignService _faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
    private readonly IFaceDetectionService _faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
   
    [Theory]
    [InlineData(112)]
    [InlineData(224)]
    [InlineData(448)]
    public void Align_ShouldReturnExpected(int size)
    {
        //arrange
        using var expected = CvInvoke.Imread($"TestData/womanAlign{size}.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var sourceFace = _faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        //act
        using var result = _faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks, size);
        //assert
        string tmpFile = $"{nameof(FaceAlignServiceTests)}womanAlignTmp{size}.jpg";
        CvInvoke.Imwrite(tmpFile, result.Align);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
