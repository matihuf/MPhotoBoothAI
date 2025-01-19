using Microsoft.EntityFrameworkCore;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Models.Entities;
using System.Drawing;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
public class DesignPrintTemplateViewModelBuilder
{
    private Mock<IFilePickerService> _filePickerService;
    private Mock<IFilesManager> _filesManager;
    private Mock<IDatabaseContext> _dbContext;
    private Mock<IApplicationInfoService> _applicationInfoService;
    private Mock<IImageManager> _imageManager;
    private Mock<IMessageBoxService> _messageBoxService;
    private List<LayoutFormatEntity> _layoutFormats;
    private List<LayoutDataEntity> _layoutDatas;
    private Mock<DbSet<LayoutFormatEntity>> _mockLayoutFormatDbSet;
    private Mock<DbSet<LayoutDataEntity>> _mockLayoutDataDbSet;

    public Mock<IFilesManager> FilesManager => _filesManager;
    public Mock<IDatabaseContext> DbContext => _dbContext;
    public Mock<IMessageBoxService> MessageBoxService => _messageBoxService;

    public DesignPrintTemplateViewModelBuilder()
    {
        _filePickerService = new Mock<IFilePickerService>();
        _filesManager = new Mock<IFilesManager>();
        _dbContext = new Mock<IDatabaseContext>();
        _applicationInfoService = new Mock<IApplicationInfoService>();
        _imageManager = new Mock<IImageManager>();
        _messageBoxService = new Mock<IMessageBoxService>();

        _layoutFormats = new List<LayoutFormatEntity>
            {
                new() { Id = 1, FormatRatio = 1.5 },
                new() { Id = 2, FormatRatio = 2.0 }
            };

        _layoutDatas = new List<LayoutDataEntity>
            {
                new() { Id = 1, PhotoLayoutData = new(), OverlayImageData = new() },
                new() { Id = 2, PhotoLayoutData = new(), OverlayImageData = new() }
            };

        _mockLayoutFormatDbSet = MockDbSet(_layoutFormats);
        _mockLayoutDataDbSet = MockDbSet(_layoutDatas);

        _dbContext.Setup(x => x.LayoutFormat).Returns(_mockLayoutFormatDbSet.Object);
        _dbContext.Setup(x => x.LayoutDatas).Returns(_mockLayoutDataDbSet.Object);
        _applicationInfoService.Setup(x => x.BackgroundDirectory).Returns("TestBackgroundDir");

        _filesManager.Setup(x => x.GetFiles(It.IsAny<string>()))
            .Returns(new List<string>());
    }

    private Mock<DbSet<T>> MockDbSet<T>(List<T> list) where T : class
    {
        var queryable = list.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        return mockSet;
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
        _messageBoxService.Setup(x => x.ShowYesNo(It.IsAny<string>(), It.IsAny<string>(), null))
            .ReturnsAsync(result);
        return this;
    }

    public DesignPrintTemplateViewModel Build()
    {
        return new DesignPrintTemplateViewModel(
            _filePickerService.Object,
            _filesManager.Object,
            _dbContext.Object,
            _applicationInfoService.Object,
            _imageManager.Object,
            _messageBoxService.Object
        );
    }

}
