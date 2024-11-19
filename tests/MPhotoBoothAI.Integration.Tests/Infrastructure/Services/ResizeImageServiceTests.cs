using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class ResizeImageServiceTests : IClassFixture<DependencyInjectionFixture>
{
    private readonly IResizeImageService _resizeImageService;

    public ResizeImageServiceTests(DependencyInjectionFixture dependencyInjectionFixture)
    {
        _resizeImageService = dependencyInjectionFixture.ServiceProvider.GetService<IResizeImageService>();
    }

    [Fact]
    public void Resize_KeepRatio_ShouldBeAsExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/womanResizedKeepRatio.dat");
        using var frame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        //act
        using var result = _resizeImageService.Resize(frame, 640, 640, true);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result.Image));
    }

    [Fact]
    public void Resize_DoNotKeepRatio_ShouldBeAsExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/womanResizedDoNotKeepRatio.dat");
        using var frame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        //act
        using var result = _resizeImageService.Resize(frame, 640, 640, false);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result.Image));
    }
}
