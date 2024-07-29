using Emgu.CV;
using MPhotoBoothAI.Infrastructure;

namespace MPhotoBoothAI.Integration.Tests;

public class ResizeImageServiceTests
{
    private readonly ResizeImageService _resizeImageService;

    public ResizeImageServiceTests()
    {
        _resizeImageService = new ResizeImageService();
    }

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
