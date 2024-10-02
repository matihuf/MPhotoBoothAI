using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceLandmarksServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void GetLandmarks_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/swapped.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        var faceLandmarksService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceLandmarksService>();
        //act
        var landmarks = faceLandmarksService.GetLandmarks(sourceFaceFrame);
        //assert
        Assert.Equal(85.91861f, landmarks[0, 0]);
        Assert.Equal(298.44702f, landmarks[0, 1]);
        Assert.Equal(39.04454f, landmarks[1, 0]);
        Assert.Equal(71.34962f, landmarks[1, 1]);
    }
}
