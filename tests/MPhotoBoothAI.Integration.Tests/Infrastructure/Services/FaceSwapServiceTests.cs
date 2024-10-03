using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceSwapServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void Swap_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/swappedLow.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        using var targetFaceFrame = RawMatFile.MatFromBase64File("TestData/woman2.dat");

        var faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        var faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
        using var sourceAlignFace = faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);

        using var targetFace = faceDetectionService.Detect(targetFaceFrame, 0.8f, 0.5f).First();
        using var targetAlignFace = faceAlignService.Align(targetFaceFrame, targetFace.Landmarks);

        var faceSwapPredictService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapPredictService>();
        using var predictResult = faceSwapPredictService.Predict(sourceAlignFace.Align, targetAlignFace.Align);

        var faceLandmarksService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceLandmarksService>();
        var predictLandmarks = faceLandmarksService.GetLandmarks(predictResult);
        var targetLandmarks = faceLandmarksService.GetLandmarks(targetAlignFace.Align);

        var faceMaskService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceMaskService>();
        using var mask = faceMaskService.GetMask(targetAlignFace.Align, predictLandmarks, targetLandmarks);
        var faceSwapService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapService>();
        //act
        using var result = faceSwapService.Swap(mask, predictResult, targetAlignFace.Norm, targetFaceFrame);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result));
    }
}
