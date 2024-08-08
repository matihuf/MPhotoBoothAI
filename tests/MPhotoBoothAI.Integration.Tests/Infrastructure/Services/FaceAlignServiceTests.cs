using Emgu.CV;
using Emgu.CV.Dnn;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceAlignServiceTests
{
    [Fact]
    public void Align_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanAlign.jpg");

        using var yoloNet = DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx");
        var faceDetectionService = new FaceDetectionService(yoloNet, new ResizeImageService());

        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();

        var faceAlignService = new FaceAlignService();
        //act
        using var result = faceAlignService.Align(sourceFace.Frame, sourceFace.Landmarks);
        //assert
        string tmpFile = "womanAlignTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
