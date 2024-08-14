using Emgu.CV;
using Emgu.CV.Dnn;
using MPhotoBoothAI.Infrastructure.Services;
using MPhotoBoothAI.Infrastructure.Services.Swap;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceSwapPredictServiceTests
{
    [Fact]
    public void Predict_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanSwapped.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");
        using var targetFaceFrame = CvInvoke.Imread("TestData/woman2.jpg");
        using var yoloNet = DnnInvoke.ReadNetFromONNX("yolov8n-face.onnx");
        var faceAlignService = new FaceAlignService();
        var faceDetectionService = new FaceDetectionService(yoloNet, new ResizeImageService());

        using var sourceFace = faceDetectionService.Detect(sourceFaceFrame, 0.8f, 0.5f).First();
        using var sourceAlignFace = faceAlignService.Align(sourceFaceFrame, sourceFace.Landmarks);

        using var targetFace = faceDetectionService.Detect(targetFaceFrame, 0.8f, 0.5f).First();
        using var targetAlignFace = faceAlignService.Align(targetFaceFrame, targetFace.Landmarks);

        using var arcfaceNet = DnnInvoke.ReadNetFromONNX("arcface_backbone.onnx");
        using var gNet = DnnInvoke.ReadNetFromONNX("G_unet_2blocks.onnx");
        var faceSwapPredictService = new FaceSwapPredictService(arcfaceNet, gNet);
        //act
        using var result = faceSwapPredictService.Predict(sourceAlignFace.Align, targetAlignFace.Align);
        //assert
        string tmpFile = "womanSwappedTmp.jpg";
        CvInvoke.Imwrite(tmpFile, result);
        using var x = CvInvoke.Imread(tmpFile);
        Assert.True(expected.Equals(x));
    }
}
