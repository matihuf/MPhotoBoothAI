using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceAlignManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void GetAlign_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanAlign224.ppm");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.ppm");
        var faceAlignManager = dependencyInjectionFixture.ServiceProvider.GetService<FaceAlignManager>();
        //act
        using var result = faceAlignManager.GetAlign(sourceFaceFrame);
        //assert
        Assert.True(expected.Equals(result.Align));
    }
}
