using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceSwapManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly FaceSwapManager _faceSwapManager = dependencyInjectionFixture.ServiceProvider.GetService<FaceSwapManager>();

    [Fact]
    public void Swap_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/swapped.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var targetFaceFrame = CvInvoke.Imread("TestData/woman2.jpg");
        //act
        using var result = _faceSwapManager.Swap(sourceFaceFrame, targetFaceFrame);
        //assert
        string tmpFile = $"{nameof(FaceSwapManagerTests)}swappedTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}