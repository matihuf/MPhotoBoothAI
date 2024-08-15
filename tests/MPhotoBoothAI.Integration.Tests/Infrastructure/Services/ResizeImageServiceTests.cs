using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class ResizeImageServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly ResizeImageService _resizeImageService = dependencyInjectionFixture.ServiceProvider.GetService<ResizeImageService>();

    [Fact]
    public void Resize_KeepRatio_ShouldBeAsExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanResizedKeepRatio.jpg");
        using var frame = CvInvoke.Imread("TestData/woman.jpg");
        //act
        var result = _resizeImageService.Resize(frame, 640, 640, true);
        //assert
        string tmpFile = "resizeKeepRatio.jpg";
        CvInvoke.Imwrite(tmpFile, result.Image);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }

    [Fact]
    public void Resize_DoNotKeepRatio_ShouldBeAsExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanResizedDoNotKeepRatio.jpg");
        using var frame = CvInvoke.Imread("TestData/woman.jpg");
        //act
        var result = _resizeImageService.Resize(frame, 640, 640, false);
        //assert
        string tmpFile = "resizeDoNotKeepRatio.jpg";
        CvInvoke.Imwrite(tmpFile, result.Image);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
