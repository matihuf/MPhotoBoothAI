using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
internal class PreviewFaceSwapTemplateViewModelBuilder
{
    private readonly Mock<ICameraManager> _cameraManager = new();
    public readonly Mock<IFaceDetectionManager> FaceDetectionManager = new();
    public readonly Mock<IFaceMultiSwapManager> FaceMultiSwapManager = new();

    public PreviewFaceSwapTemplateViewModel Build() => new(FaceMultiSwapManager.Object, _cameraManager.Object, FaceDetectionManager.Object);
}
