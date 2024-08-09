using Emgu.CV;
using Emgu.CV.Dnn;
using MPhotoBoothAI.Infrastructure;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceSwapServiceTests
{
    [Fact]
    public void Swap_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanSwapped.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var targetFaceFrame = CvInvoke.Imread("TestData/woman2.jpg");
        using var yoloNet = DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx");
        var faceAlignService = new FaceAlignService();
        var faceDetectionService = new FaceDetectionService(yoloNet, new ResizeImageService());

        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        using var sourceAlignFace = faceAlignService.Align(sourceFace.Frame, sourceFace.Landmarks);

        using var targetFace = faceDetectionService.Detect(targetFaceFrame, 0.8f, 0.5f).First();
        using var targetAlignFace = faceAlignService.Align(targetFace.Frame, targetFace.Landmarks);

        using var arcfaceNet = DnnInvoke.ReadNetFromONNX("arcface_backbone.onnx");
        using var gNet = DnnInvoke.ReadNetFromONNX("G_unet_2blocks.onnx");
        var faceSwapService = new FaceSwapService(arcfaceNet, gNet);
        //act
        using var result = faceSwapService.Swap(sourceAlignFace, targetAlignFace);
        //assert
        string tmpFile = "womanSwappedTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
