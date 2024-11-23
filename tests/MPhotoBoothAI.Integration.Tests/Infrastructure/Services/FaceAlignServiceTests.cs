using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceAlignServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Theory]
    [InlineData(112)]
    [InlineData(224)]
    [InlineData(448)]
    public void Align_ShouldReturnExpected(int size)
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File($"TestData/womanAlign{size}.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        var faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        var faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
        //act
        using var result = faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks, size);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result.Align));
    }
}
