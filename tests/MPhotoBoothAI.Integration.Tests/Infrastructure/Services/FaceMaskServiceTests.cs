using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceMaskServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void GetMask_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/mask.dat");
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
        //act
        using var result = faceMaskService.GetMask(targetAlignFace.Align, predictLandmarks, targetLandmarks);
        using var resultDeNormalized = DeNormalize(result);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, resultDeNormalized));
    }

    private static Mat DeNormalize(Mat normalizedFace)
    {
        var denormalizedImg = new Mat();
        normalizedFace.ConvertTo(denormalizedImg, DepthType.Cv8U, 255f, 0);
        return denormalizedImg;
    }
}
