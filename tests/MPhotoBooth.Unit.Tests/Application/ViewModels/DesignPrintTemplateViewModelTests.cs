using DynamicData;
using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using System.Drawing;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels;
public class DesignPrintTemplateViewModelTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        var viewModel = new DesignPrintTemplateViewModelBuilder().Build();

        Assert.Equal(1, viewModel.Id);
        Assert.NotNull(viewModel.LayoutFormat);
        Assert.NotNull(viewModel.BackgroundInfo);
    }

    [Fact]
    public async Task AddBackgroundToList_WithValidImage_CopiesFileAndUpdatesBackgroundList()
    {
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithFilePicker("testFile.jpg")
            .WithImageSize("testFile.jpg", new Size(100, 150))
            .WithBackgroundFiles("TestBackgroundDir\\1", new List<string> { "existing.jpg" });

        var viewModel = builder.Build();

        await viewModel.AddBackgroundToListCommand.ExecuteAsync(null);

        builder.FilesManager.Verify(x => x.CopyFile("testFile.jpg", It.IsAny<string>()), Times.Once);
        builder.FilesManager.Verify(x => x.GetFiles(It.IsAny<string>()), Times.AtLeast(2));
    }

    [Fact]
    public async Task ChangeFormatIndex_WithUnsavedChanges_ShowsConfirmationDialog()
    {
        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithMessageBoxResult(true);

        var viewModel = builder.Build();
        viewModel.NotSavedChange = true;

        await viewModel.ChangeFormatIndexCommand.ExecuteAsync(2);

        builder.MessageBoxService.Verify(
            x => x.ShowYesNo(It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Once);
        Assert.Equal(2, viewModel.Id);
    }

    [Fact]
    public void RemoveBackgroundFromList_RemovesSelectedBackground()
    {
        var backgroundFiles = new List<string> { "bg1.jpg", "bg2.jpg" };
        var formatDir = Path.Combine("TestBackgroundDir", "1");

        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithBackgroundFiles(formatDir, backgroundFiles);

        var viewModel = builder.Build();

        viewModel.Id = 1;

        viewModel.BackgroundInfo.BackgroundPathsList.AddRange(backgroundFiles);
        viewModel.BackgroundInfo.SelectedItem = "bg1.jpg";
        viewModel.BackgroundInfo.BackgroundPath = "bg1.jpg";

        viewModel.RemoveBackgroundFromListCommand.Execute(null);

        builder.FilesManager.Verify(x => x.DeleteFile("bg1.jpg"), Times.Once);
        Assert.DoesNotContain("bg1.jpg", viewModel.BackgroundInfo.BackgroundPathsList);
        Assert.Equal("bg2.jpg", viewModel.BackgroundInfo.BackgroundPath);
    }

    [Fact]
    public void RemoveBackgroundFromList_WhenLastBackground_ClearsBackgroundPath()
    {
        var backgroundFiles = new List<string> { "bg1.jpg" };
        var formatDir = Path.Combine("TestBackgroundDir", "1");

        var builder = new DesignPrintTemplateViewModelBuilder()
            .WithBackgroundFiles(formatDir, backgroundFiles);

        var viewModel = builder.Build();

        viewModel.Id = 1;

        viewModel.BackgroundInfo.BackgroundPathsList.AddRange(backgroundFiles);
        viewModel.BackgroundInfo.SelectedItem = "bg1.jpg";
        viewModel.BackgroundInfo.BackgroundPath = "bg1.jpg";

        viewModel.RemoveBackgroundFromListCommand.Execute(null);

        builder.FilesManager.Verify(x => x.DeleteFile("bg1.jpg"), Times.Once);
        Assert.Empty(viewModel.BackgroundInfo.BackgroundPathsList);
        Assert.Null(viewModel.BackgroundInfo.BackgroundPath);
    }

    [Fact]
    public async Task SaveLayout_SavesChangesAndShowsConfirmation()
    {
        var builder = new DesignPrintTemplateViewModelBuilder();
        var viewModel = builder.Build();
        viewModel.NotSavedChange = true;

        await viewModel.SaveLayoutCommand.ExecuteAsync(null);

        builder.DbContext.Verify(x => x.SaveChanges(), Times.Once);
        builder.MessageBoxService.Verify(
            x => x.ShowInfo(It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Once);
        Assert.False(viewModel.NotSavedChange);
    }
}
