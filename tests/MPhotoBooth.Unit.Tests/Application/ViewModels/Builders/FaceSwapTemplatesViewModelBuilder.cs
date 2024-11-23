using Moq;
using MPhotoBoothAI.Application.Assets;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
public class FaceSwapTemplatesViewModelBuilder
{
    private readonly Mock<IMessageBoxService> MessageBoxService = new();

    public FaceSwapTemplatesViewModel Build(IDatabaseContext databaseContext) => new(databaseContext, MessageBoxService.Object);

    internal FaceSwapTemplatesViewModelBuilder WithDeleteGroupConfirmation(bool confirmed)
    {
        MessageBoxService.Setup(x => x.ShowYesNo(UI.deleteGroup, UI.deleteGroupDesc, It.IsAny<IMainWindow>())).ReturnsAsync(confirmed);
        return this;
    }

    internal FaceSwapTemplatesViewModelBuilder WithGroupName(string groupName)
    {
        MessageBoxService.Setup(x => x.ShowInput(UI.addGroup, UI.name, It.IsAny<IMainWindow>())).ReturnsAsync(groupName);
        return this;
    }
}
