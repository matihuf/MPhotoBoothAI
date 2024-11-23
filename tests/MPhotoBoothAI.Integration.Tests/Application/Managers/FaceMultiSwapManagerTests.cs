using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceMultiSwapManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void Swap_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/multiswap.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/bradangelina.dat");
        using var targetFaceFrame = RawMatFile.MatFromBase64File("TestData/couple.dat");
        var faceMultiSwapManager = dependencyInjectionFixture.ServiceProvider.GetService<IFaceMultiSwapManager>();
        //act
        using var result = faceMultiSwapManager.Swap(sourceFaceFrame, targetFaceFrame);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result));
    }
}