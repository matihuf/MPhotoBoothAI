using Emgu.CV;
using Emgu.CV.CvEnum;
using Moq;
using MPhotoBooth.Unit.Tests.Application.Managers.Builders;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBooth.Unit.Tests.Application.Managers;

public class FaceSwapManagerTests
{
    private readonly FaceSwapManagerBuilder _builder;

    public FaceSwapManagerTests()
    {
        _builder = new FaceSwapManagerBuilder();
    }

    [Fact]
    public void Swap_CallServicesInOrder()
    {
        //arrange
        using var sourceAlign = new FaceAlign(new Mat(), new Mat());
        using var target = new Mat(2, 2, DepthType.Default, 3);
        using var targetAlign = new FaceAlign(new Mat(), new Mat());

        var sequence = new MockSequence();
        _builder.FaceSwapPredictService.InSequence(sequence).Setup(x => x.Predict(It.IsAny<Mat>(), It.IsAny<Mat>()));
        _builder.FaceEnhancerService.InSequence(sequence).Setup(x => x.Enhance(It.IsAny<Mat>()));
        _builder.FaceMaskManager.InSequence(sequence).Setup(x => x.GetMask(It.IsAny<Mat>(), It.IsAny<Mat>()));
        _builder.FaceSwapService.InSequence(sequence).Setup(x => x.Swap(It.IsAny<Mat>(), It.IsAny<Mat>(), It.IsAny<Mat>(), It.IsAny<Mat>()));

        var manager = _builder.Build();
        //act
        using var swapped = manager.Swap(sourceAlign, targetAlign, target);
        //assert
        _builder.FaceSwapPredictService.Verify(x => x.Predict(sourceAlign.Align, targetAlign.Align));
        _builder.FaceEnhancerService.Verify(x => x.Enhance(It.IsAny<Mat>()));
        _builder.FaceMaskManager.Verify(x => x.GetMask(It.IsAny<Mat>(), It.IsAny<Mat>()));
        _builder.FaceSwapService.Verify(x => x.Swap(It.IsAny<Mat>(), It.IsAny<Mat>(), It.IsAny<Mat>(), It.IsAny<Mat>()));
    }
}
