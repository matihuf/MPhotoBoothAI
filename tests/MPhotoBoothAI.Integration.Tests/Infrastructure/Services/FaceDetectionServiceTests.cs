using Emgu.CV;
using Emgu.CV.Dnn;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceDetectionServiceTests
{
    [Fact]
    public void Detec_ShouldReturnOneFace()
    {
        //arrange
        using var net = DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx");
        using var frame = CvInvoke.Imread("TestData/woman.jpg");
        var faceDetectionService = new FaceDetectionService(net, new ResizeImageService());
        //act
        var faces = faceDetectionService.Detect(frame, 0.8f, 0.5f);
        //assert
        Assert.Single(faces);
    }
}
