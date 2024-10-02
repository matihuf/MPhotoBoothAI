using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceLandmarksServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void GetLandmarks_ShouldReturnExpected()
    {
        //arrange
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        var faceLandmarksService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceLandmarksService>();
        //act
        var landmarks = faceLandmarksService.GetLandmarks(sourceFaceFrame);
        //assert
        Assert.Equal(Math.Round(85.91861f, 2), Math.Round(landmarks[0, 0], 2));
        Assert.Equal(Math.Round(298.44702f, 2), Math.Round(landmarks[0, 1], 2));
        Assert.Equal(Math.Round(39.04454f, 2), Math.Round(landmarks[1, 0], 2));
        Assert.Equal(Math.Round(71.34962f, 2), Math.Round(landmarks[1, 1], 2));
    }
}
