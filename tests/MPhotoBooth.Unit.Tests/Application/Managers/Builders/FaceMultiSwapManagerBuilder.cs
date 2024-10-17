using Emgu.CV;
using Microsoft.Extensions.Logging;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Managers;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBooth.Unit.Tests.Application.Managers.Builders;

public class FaceMultiSwapManagerBuilder
{
    public readonly Mock<IFaceAlignManager> FaceAlignManager = new();
    public readonly Mock<IFaceSwapManager> FaceSwapManager = new();
    private readonly Mock<ILogger<FaceMultiSwapManager>> _logger = new();

    public IFaceMultiSwapManager Build() => new FaceMultiSwapManager(FaceAlignManager.Object, FaceSwapManager.Object, _logger.Object);

    internal FaceMultiSwapManagerBuilder WithAligns(Mat source, FaceAlignDetails[] alignDetails)
    {
        FaceAlignManager.Setup(x => x.GetAligns(It.Is<Mat>(m => m.Width == source.Width))).Returns(alignDetails);
        return this;
    }
}
