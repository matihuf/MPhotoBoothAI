using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceDetectionServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    [Fact]
    public void Detec_ShouldReturnOneFace()
    {
        //arrange
        using var frame = RawMatFile.MatFromBase64File("TestData/woman.dat");
        var faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();
        //act
        var faces = faceDetectionService.Detect(frame, 0.8f, 0.5f);
        //assert
        Assert.Single(faces);
        faces.ElementAt(0).Dispose();
    }
}
