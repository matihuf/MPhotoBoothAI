using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Models.Enums;
using System.Drawing;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
public class DesignPrintTemplateViewModelBuilder
{
    private Mock<IApplicationInfoService> _applicationInfoService = new();
    public Mock<IImageManager> ImageManager = new();
    public Mock<IFilesManager> FilesManager = new();
    public Mock<IFilePickerService> FilePickerService = new();
    public Mock<IMessageBoxService> MessageBoxService = new();

    public DesignPrintTemplateViewModelBuilder()
    {
        _applicationInfoService.Setup(x => x.BackgroundDirectory).Returns("TestBackgroundDir");
        FilesManager.Setup(x => x.GetFiles(It.IsAny<string>()))
            .Returns(new List<string>());
    }

    public DesignPrintTemplateViewModelBuilder WithMessageBoxResult(bool result)
    {
        MessageBoxService.Setup(x => x.ShowYesNo(MPhotoBoothAI.Application.Assets.UI.notSavedChangesTittle, MPhotoBoothAI.Application.Assets.UI.notSavedChangesMessage, null))
            .ReturnsAsync(result);
        return this;
    }

    public DesignPrintTemplateViewModelBuilder WithPickedFilePath(string? filePath)
    {
        FilePickerService
            .Setup(x => x.PickFilePath(It.IsAny<FilePickerFileType[]>()))
            .ReturnsAsync(filePath);
        return this;
    }

    public DesignPrintTemplateViewModelBuilder WithImageSize(Size? size)
    {
        ImageManager.Setup(x => x.GetImageSizeFromFile(It.IsAny<string>()))
            .Returns(size);
        return this;
    }

    public DesignPrintTemplateViewModel Build(IDatabaseContext databaseContext)
    {
        return new DesignPrintTemplateViewModel(
            FilePickerService.Object,
            FilesManager.Object,
            databaseContext,
            _applicationInfoService.Object,
            ImageManager.Object,
            MessageBoxService.Object
        );
    }
}
