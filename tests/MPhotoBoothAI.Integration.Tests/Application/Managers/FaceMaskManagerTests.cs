using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Integration.Tests.Application.Managers;

public class FaceMaskManagerTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly FaceMaskManager _faceMaskManager = dependencyInjectionFixture.ServiceProvider.GetService<FaceMaskManager>();
    private readonly IFaceDetectionService _faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
    private readonly IFaceAlignService _faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
    private readonly IFaceSwapPredictService _faceSwapPredictService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapPredictService>();

    [Fact]
    public void GetMask_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/mask.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var targetFaceFrame = CvInvoke.Imread("TestData/woman2.jpg");

        using var sourceFace = _faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        using var sourceAlignFace = _faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);

        using var targetFace = _faceDetectionService.Detect(targetFaceFrame, 0.8f, 0.5f).First();
        using var targetAlignFace = _faceAlignService.Align(targetFaceFrame, targetFace.Landmarks);

        using var predictResult = _faceSwapPredictService.Predict(sourceAlignFace.Align, targetAlignFace.Align);

        //act
        using var result = _faceMaskManager.GetMask(targetAlignFace.Align, predictResult);
        using var resultDeNormalized = DeNormalize(result);
        //assert
        string tmpFile =  $"{nameof(FaceMaskManagerTests)}maskTmp.jpg";
        CvInvoke.Imwrite(tmpFile, resultDeNormalized);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }

    private static Mat DeNormalize(Mat normalizedFace)
    {
        var denormalizedImg = new Mat();
        normalizedFace.ConvertTo(denormalizedImg, DepthType.Cv8U, 255f, 0);
        return denormalizedImg;
    }
}
