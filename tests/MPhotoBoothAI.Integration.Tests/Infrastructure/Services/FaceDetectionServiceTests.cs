using Emgu.CV;
using Emgu.CV.Dnn;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceDetectionServiceTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>
{
    private readonly IFaceDetectionService _faceDetectionService = dependencyInjectionFixture.ServiceProvider.GetService<IFaceDetectionService>();

    [Fact]
    public void Detec_ShouldReturnOneFace()
    {
        //arrange
        using var net = DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx");
        using var frame = CvInvoke.Imread("TestData/woman.jpg");
        //act
        var faces = _faceDetectionService.Detect(frame, 0.8f, 0.5f);
        //assert
        Assert.Single(faces);
        faces.ElementAt(0).Dispose();
    }
}
