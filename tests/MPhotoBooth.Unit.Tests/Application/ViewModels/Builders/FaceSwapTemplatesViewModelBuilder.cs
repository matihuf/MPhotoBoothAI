using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
public class FaceSwapTemplatesViewModelBuilder
{
    private readonly Mock<IMessageBoxService> MessageBoxService = new();

    public FaceSwapTemplatesViewModel Build(IDatabaseContext databaseContext) => new(databaseContext, MessageBoxService.Object);

    internal FaceSwapTemplatesViewModelBuilder WithDeleteGroupConfirmation(bool confirmed)
    {
        MessageBoxService.Setup(x => x.ShowYesNo(MPhotoBoothAI.Application.Assets.UI.deleteGroup, MPhotoBoothAI.Application.Assets.UI.deleteGroupDesc)).ReturnsAsync(confirmed);
        return this;
    }

    internal FaceSwapTemplatesViewModelBuilder WithGroupName(string groupName)
    {
        MessageBoxService.Setup(x => x.ShowInput(MPhotoBoothAI.Application.Assets.UI.addGroup, MPhotoBoothAI.Application.Assets.UI.name)).ReturnsAsync(groupName);
        return this;
    }
}
