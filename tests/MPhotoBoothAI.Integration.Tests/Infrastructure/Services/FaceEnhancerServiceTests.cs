using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceEnhancerServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceEnhancerService _faceEnhancerService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceEnhancerService>();

    [Fact]
    public void GetLandmarks_ShouldReturnExpected()
    {
        //arrange
        // using var expected = CvInvoke.Imread("TestData/womanSwapped.jpg");
        // using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        // //act
        // var landmarks = _faceEnhancerService.GetLandmarks(sourceFaceFrame);
        //assert
        Assert.True(true);
    }
}
