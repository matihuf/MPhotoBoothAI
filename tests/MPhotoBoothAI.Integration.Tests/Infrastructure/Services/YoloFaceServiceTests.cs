using Emgu.CV;
using Emgu.CV.Dnn;
using MPhotoBoothAI.Infrastructure;

namespace MPhotoBoothAI.Integration.Tests;

public class YoloFaceServiceTests
{
    [Fact]
    public void YoloFace_Should_Return()
    {
        //arrange
        using var net = DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx");
        using var frame = CvInvoke.Imread("TestData/woman.jpg");
        var yoloFace = new YoloFaceService(net, new ResizeImageService());
        //act
        yoloFace.Run(frame, 0.45f, 0.5f);
    }
}
