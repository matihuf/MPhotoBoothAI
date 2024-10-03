using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceAlignManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void GetAlign_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/womanAlign224.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        var faceAlignManager = dependencyInjectionFixture.ServiceProvider.GetService<FaceAlignManager>();
        //act
        using var result = faceAlignManager.GetAlign(sourceFaceFrame);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result.Align));
    }
}
