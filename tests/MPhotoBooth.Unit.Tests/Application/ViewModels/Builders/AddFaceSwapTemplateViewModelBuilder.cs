using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
internal class AddFaceSwapTemplateViewModelBuilder
{
    private readonly Mock<ICameraManager> _cameraManager = new();
    public readonly Mock<IAddFaceSwapTemplateManager> AddFaceSwapTemplateManager = new();
    public readonly Mock<IFaceMultiSwapManager> FaceMultiSwapManager = new();

    public AddFaceSwapTemplateViewModel Build() => new(AddFaceSwapTemplateManager.Object, FaceMultiSwapManager.Object, _cameraManager.Object);

    internal AddFaceSwapTemplateViewModelBuilder WithFaceSwapTemplate(FaceSwapTemplate faceSwapTemplate)
    {
        AddFaceSwapTemplateManager.Setup(x => x.PickTemplate()).ReturnsAsync(faceSwapTemplate);
        return this;
    }

    internal AddFaceSwapTemplateViewModelBuilder WithSaveTemplate(int templateId)
    {
        AddFaceSwapTemplateManager.Setup(x => x.SaveTemplate(It.IsAny<int>(), It.IsAny<FaceSwapTemplate>())).Returns(templateId);
        return this;
    }
}
