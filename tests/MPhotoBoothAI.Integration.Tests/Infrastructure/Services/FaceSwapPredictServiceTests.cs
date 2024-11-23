using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceSwapPredictServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void Predict_ShouldReturnExpected()
    {
        //arrange
        using var expected = RawMatFile.MatFromBase64File("TestData/predict.dat");
        using var sourceFaceFrame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        using var targetFaceFrame = RawMatFile.MatFromBase64File("TestData/woman2.dat");

        var faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        var faceAlignService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceAlignService>();
        using var sourceAlignFace = faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);

        using var targetFace = faceDetectionService.Detect(targetFaceFrame, 0.8f, 0.5f).First();
        using var targetAlignFace = faceAlignService.Align(targetFaceFrame, targetFace.Landmarks);
        var faceSwapPredictService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceSwapPredictService>();
        //act
        using var result = faceSwapPredictService.Predict(sourceAlignFace.Align, targetAlignFace.Align);
        //assert
        Assert.True(RawMatFile.RawEqual(expected, result));
    }
}
