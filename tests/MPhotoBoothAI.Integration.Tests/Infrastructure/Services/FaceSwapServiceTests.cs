using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceSwapServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceMaskService _faceMaskService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceMaskService>();
    private readonly IFaceDetectionService _faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
    private readonly IFaceAlignService _faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
    private readonly IFaceSwapPredictService _faceSwapPredictService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapPredictService>();
    private readonly IFaceLandmarksService _faceLandmarksService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceLandmarksService>();
    private readonly IFaceSwapService _faceSwapService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapService>();

    [Fact]
    public void Swap_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/swapped.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var targetFaceFrame = CvInvoke.Imread("TestData/woman2.jpg");

        using var sourceFace = _faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        using var sourceAlignFace = _faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);

        using var targetFace = _faceDetectionService.Detect(targetFaceFrame, 0.8f, 0.5f).First();
        using var targetAlignFace = _faceAlignService.Align(targetFaceFrame, targetFace.Landmarks);

        using var predictResult = _faceSwapPredictService.Predict(sourceAlignFace.Align, targetAlignFace.Align);

        var predictLandmarks = _faceLandmarksService.GetLandmarks(predictResult);
        var targetLandmarks = _faceLandmarksService.GetLandmarks(targetAlignFace.Align);

        using var mask = _faceMaskService.GetMask(targetAlignFace.Align, predictLandmarks, targetLandmarks);
        //act
        using var result = _faceSwapService.Swap(mask, predictResult, targetAlignFace.Norm, targetFaceFrame);
        //assert
        string tmpFile = $"{nameof(FaceSwapServiceTests)}swappedTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
