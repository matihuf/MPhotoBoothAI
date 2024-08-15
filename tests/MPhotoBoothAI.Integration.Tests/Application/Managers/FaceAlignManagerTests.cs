using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceAlignManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly FaceAlignManager _faceAlignManager = dependencyInjectionFixture.ServiceProvider.GetService<FaceAlignManager>();
   
    [Fact]
    public void GetAlign_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanAlign.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        //act
        using var result = _faceAlignManager.GetAlign(sourceFaceFrame);
        //assert
        string tmpFile = $"{nameof(FaceAlignManagerTests)}womanAlignTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result.Align);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
