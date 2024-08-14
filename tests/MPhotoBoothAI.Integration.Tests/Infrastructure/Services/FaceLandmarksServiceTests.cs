using Emgu.CV;
using Emgu.CV.Dnn;
using MPhotoBoothAI.Infrastructure.Services;

namespace MPhotoBoothAI.Integration.Tests.Infrastructure.Services;

public class FaceLandmarksServiceTests
{
    [Fact]
    public void GetLandmarks_ShouldReturnExpected()
    {
        //arrange
        using var expected = CvInvoke.Imread("TestData/womanSwapped.jpg");
        using var sourceFaceFrame = CvInvoke.Imread("TestData/woman.jpg");

        using var lNet = DnnInvoke.ReadNetFromONNX("face_landmarks.onnx");
        var faceLandmarksService = new FaceLandmarksService(lNet);
        //act
        var landmarks = faceLandmarksService.GetLandmarks(sourceFaceFrame);
        //assert
        Assert.Equal(85.91861f, landmarks[0, 0]);
        Assert.Equal(298.44702f, landmarks[0, 1]);
        Assert.Equal(39.04454f, landmarks[1, 0]);
        Assert.Equal(71.34962f, landmarks[1, 1]);
    }
}
