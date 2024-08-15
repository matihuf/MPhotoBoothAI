using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceLandmarksServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceLandmarksService _faceLandmarksService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceLandmarksService>();

    [Fact]
    public void GetLandmarks_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanSwapped.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        //act
        var landmarks = _faceLandmarksService.GetLandmarks(sourceFaceFrame);
        //assert
        Assert.Equal(85.91861f, landmarks[0, 0]);
        Assert.Equal(298.44702f, landmarks[0, 1]);
        Assert.Equal(39.04454f, landmarks[1, 0]);
        Assert.Equal(71.34962f, landmarks[1, 1]);
    }
}
