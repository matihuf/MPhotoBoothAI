using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.Enums;
using System.Drawing;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels;
public class DesignPrintTemplateViewModelTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var viewModel = new DesignPrintTemplateViewModelBuilder().Build(databaseContext);

        //assert
        Assert.Equal(1, viewModel.Id);
        Assert.NotNull(viewModel.LayoutFormat);
    }

    [Fact]
    public async Task ChangeFormatIndex_WithUnsavedChanges_ShowsConfirmationDialog()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithMessageBoxResult(true);
        var viewModel = builder.Build(databaseContext);
        viewModel.NotSavedChange = true;

        //act
        await viewModel.ChangeFormatIndexCommand.ExecuteAsync(2);

        //assert
        builder.MessageBoxService.Verify(
            x => x.ShowYesNo(MPhotoBoothAI.Application.Assets.UI.notSavedChangesTittle, MPhotoBoothAI.Application.Assets.UI.notSavedChangesMessage, null),
            Times.Once);
        Assert.Equal(2, viewModel.Id);
    }

    [Fact]
    public async Task SaveLayout_ShowsConfirmation()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder();
        var viewModel = builder.Build(databaseContext);
        viewModel.NotSavedChange = true;
        viewModel.SelectedLayoutFormat = new LayoutFormatEntity { Id = 1, FormatRatio = 1.5 }; ;
        viewModel.SelectedLayoutData = new LayoutDataEntity { Id = 1, PhotoLayoutData = new(), OverlayImageData = new() }; ;

        //act
        await viewModel.SaveLayoutCommand.ExecuteAsync(null);

        //assert
        builder.MessageBoxService.Verify(
            x => x.ShowInfo(MPhotoBoothAI.Application.Assets.UI.savedChanges, MPhotoBoothAI.Application.Assets.UI.savedChanges, null),
            Times.Once);
        Assert.False(viewModel.NotSavedChange);
    }

    [Fact]
    public async Task SaveLayout_DatabaseShouldChange()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder();
        var viewModel = builder.Build(databaseContext);
        viewModel.NotSavedChange = true;
        viewModel.SelectedLayoutFormat = new LayoutFormatEntity { Id = 1, FormatRatio = 1.5 }; ;
        viewModel.SelectedLayoutData = new LayoutDataEntity { Id = 1, PhotoLayoutData = new(), OverlayImageData = new() };

        //act
        await viewModel.SaveLayoutCommand.ExecuteAsync(null);
        var layoutData = databaseContext.LayoutDatas.FirstOrDefault();

        //assert
        Assert.NotNull(layoutData);
    }

    [Fact]
    public async Task SetBackground_WhenImageSizeIsNull_ShouldReturnEarly()
    {
        // arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithImageSize(null)
            .WithPickedFilePath("test.jpg");
        var viewModel = builder.Build(databaseContext);

        // act
        await viewModel.SetBackgroundCommand.ExecuteAsync(null);

        // assert
        builder.FilePickerService.Verify(x => x.PickFilePath(It.IsAny<FilePickerFileType[]>()), Times.Once);
        builder.FilesManager.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SetBackground_WhenRatioIsIncorrectAndUserDeclines_ShouldNotCopyFile()
    {
        // arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithImageSize(new Size(100, 200))
            .WithPickedFilePath("test.jpg");

        builder.MessageBoxService
            .Setup(x => x.ShowYesNo(
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioTitle,
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioMessage,
                It.IsAny<IWindow>()))
            .ReturnsAsync(false);

        var viewModel = builder.Build(databaseContext);
        viewModel.SelectedLayoutFormat = new LayoutFormatEntity { Id = 1, FormatRatio = 1.5 };

        // act
        await viewModel.SetBackgroundCommand.ExecuteAsync(null);

        // assert
        builder.MessageBoxService.Verify(
            x => x.ShowYesNo(
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioTitle,
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioMessage,
                It.IsAny<IWindow>()),
            Times.Once);
        builder.FilesManager.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SetBackground_WhenRatioIsIncorrectButUserAccepts_ShouldCopyFile()
    {
        // arrange
        const string pickedFile = "test.jpg";
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithImageSize(new Size(100, 200))
            .WithPickedFilePath(pickedFile);

        builder.MessageBoxService
            .Setup(x => x.ShowYesNo(
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioTitle,
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioMessage,
                It.IsAny<IWindow>()))
            .ReturnsAsync(true);

        var viewModel = builder.Build(databaseContext);
        viewModel.SelectedLayoutFormat = new LayoutFormatEntity { Id = 1, FormatRatio = 1.5 };

        // act
        await viewModel.SetBackgroundCommand.ExecuteAsync(null);

        // assert
        builder.MessageBoxService.Verify(
            x => x.ShowYesNo(
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioTitle,
                MPhotoBoothAI.Application.Assets.UI.wrongImageRatioMessage,
                It.IsAny<IWindow>()),
            Times.Once);
        builder.FilesManager.Verify(x => x.CopyFile(pickedFile, It.IsAny<string>(), "background.jpg"), Times.Once);
    }

    [Fact]
    public async Task SetBackground_WhenRatioIsCorrect_ShouldCopyFileWithoutWarning()
    {
        // arrange
        string pickedFile = "test.jpg";
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithImageSize(new Size(200, 300))
            .WithPickedFilePath(pickedFile);

        var viewModel = builder.Build(databaseContext);
        viewModel.SelectedLayoutFormat = new LayoutFormatEntity { Id = 1, FormatRatio = 1.5 };

        // act
        await viewModel.SetBackgroundCommand.ExecuteAsync(null);

        // assert
        builder.MessageBoxService.Verify(
            x => x.ShowYesNo(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IWindow>()),
            Times.Never);
        builder.FilesManager.Verify(x => x.CopyFile(pickedFile, It.IsAny<string>(), "background.jpg"), Times.Once);
        Assert.NotNull(viewModel.BackgroundPath);
    }

    [Fact]
    public async Task SetBackground_WhenFilePickerReturnsNull_ShouldReturnEarly()
    {
        // arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithPickedFilePath(null);
        var viewModel = builder.Build(databaseContext);

        // act
        await viewModel.SetBackgroundCommand.ExecuteAsync(null);

        // assert
        builder.ImageManager.Verify(x => x.GetImageSizeFromFile(It.IsAny<string>()), Times.Never);
        builder.FilesManager.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
