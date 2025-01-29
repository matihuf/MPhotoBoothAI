using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Application.ViewModels;
using System.Drawing;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
public class DesignPrintTemplateViewModelBuilder
{
    private Mock<IFilePickerService> _filePickerService = new();
    private Mock<IApplicationInfoService> _applicationInfoService = new();
    private Mock<IImageManager> _imageManager = new();
    private Mock<IFilesManager> _filesManager = new();
    public Mock<IMessageBoxService> MessageBoxService = new();

    public DesignPrintTemplateViewModelBuilder()
    {
        _applicationInfoService.Setup(x => x.BackgroundDirectory).Returns("TestBackgroundDir");
        _filesManager.Setup(x => x.GetFiles(It.IsAny<string>()))
            .Returns(new List<string>());
    }

    public DesignPrintTemplateViewModelBuilder WithFilePicker(string returnPath)
    {
        _filePickerService.Setup(x => x.PickFilePath(It.IsAny<FileTypes>()))
            .ReturnsAsync(returnPath);
        return this;
    }

    public DesignPrintTemplateViewModelBuilder WithBackgroundFiles(string directory, List<string> files)
    {
        _filesManager.Setup(x => x.GetFiles(directory)).Returns(files);

        foreach (FormatTypes format in Enum.GetValues(typeof(FormatTypes)))
        {
            var formatDir = Path.Combine("TestBackgroundDir", format.ToString());
            if (formatDir == directory)
            {
                _filesManager.Setup(x => x.GetFiles(formatDir)).Returns(files);
            }
            else
            {
                _filesManager.Setup(x => x.GetFiles(formatDir)).Returns(new List<string>());
            }
        }
        return this;
    }

    public DesignPrintTemplateViewModelBuilder WithImageSize(string path, Size? size)
    {
        _imageManager.Setup(x => x.GetImageSizeFromFile(path)).Returns(size);
        return this;
    }

    public DesignPrintTemplateViewModelBuilder WithMessageBoxResult(bool result)
    {
        MessageBoxService.Setup(x => x.ShowYesNo(It.IsAny<string>(), It.IsAny<string>(), null))
            .ReturnsAsync(result);
        return this;
    }

    public DesignPrintTemplateViewModel Build(IDatabaseContext databaseContext)
    {
        return new DesignPrintTemplateViewModel(
            _filePickerService.Object,
            _filesManager.Object,
            databaseContext,
            _applicationInfoService.Object,
            _imageManager.Object,
            MessageBoxService.Object
        );
    }

}
