using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceSwapManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void Swap_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/swapped.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        using var targetFaceFrame = RawMatFile.MatFromBase64File("TestData/woman2.dat");
        var faceAlignManager = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignManager>();
        using var sourceAlign = faceAlignManager.GetAlign(sourceFaceFrame);
        using var targetAlign = faceAlignManager.GetAlign(targetFaceFrame);
        var faceSwapManager = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapManager>();
        //act
        using var result = faceSwapManager.Swap(sourceAlign, targetAlign, targetFaceFrame);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result));
    }
}