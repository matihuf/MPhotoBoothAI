using Moq;
using MPhotoBoothAI.Application.Assets;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
public class FaceSwapGroupTemplatesViewModelBuilder
{
    private readonly Mock<IWindowService> WindowService = new();
    public readonly Mock<IMessageBoxService> MessageBoxService = new();
    public readonly Mock<IFaceSwapTemplateFileManager> FaceSwapTemplateFileService = new();

    public FaceSwapGroupTemplatesViewModel Build(IDatabaseContext databaseContext) => new(databaseContext, MessageBoxService.Object, WindowService.Object,
        FaceSwapTemplateFileService.Object);

    internal FaceSwapGroupTemplatesViewModelBuilder WithDeleteGroupConfirmation(bool confirmed)
    {
        MessageBoxService.Setup(x => x.ShowYesNo(UI.deleteQuestion, UI.deleteGroupDesc, It.IsAny<IWindow>())).ReturnsAsync(confirmed);
        return this;
    }

    internal FaceSwapGroupTemplatesViewModelBuilder WithGroupName(string groupName)
    {
        MessageBoxService.Setup(x => x.ShowInput(UI.addGroup, UI.name, It.IsAny<IWindow>())).ReturnsAsync(groupName);
        return this;
    }

    internal FaceSwapGroupTemplatesViewModelBuilder WithDeleteTemplateConfirmation(bool confirmed)
    {
        MessageBoxService.Setup(x => x.ShowYesNo(UI.deleteQuestion, UI.deleteTemplate, It.IsAny<IWindow>())).ReturnsAsync(confirmed);
        return this;
    }

    internal FaceSwapGroupTemplatesViewModelBuilder WithFullTemplateThumbnailPath(string path)
    {
        FaceSwapTemplateFileService.Setup(x => x.GetFullTemplateThumbnailPath(It.IsAny<int>(), It.IsAny<int>())).Returns(path);
        return this;
    }
}
