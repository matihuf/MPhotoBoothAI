using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using MPhotoBoothAI.Application.Models;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.Entities;

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
            x => x.ShowYesNo(It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Once);
        Assert.Equal(2, viewModel.Id);
    }

    [Fact]
    public async Task SaveLayout_SavesChangesAndShowsConfirmation()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var builder = new DesignPrintTemplateViewModelBuilder();
        var viewModel = builder.Build(databaseContext);
        viewModel.NotSavedChange = true;

        var layoutFormat = new LayoutFormatEntity { Id = 1, FormatRatio = 1.5 };

        var layoutData = new LayoutDataEntity { Id = 1, PhotoLayoutData = new(), OverlayImageData = new() };

        //act
        viewModel.SelectedLayoutFormat = layoutFormat;
        viewModel.SelectedLayoutData = layoutData;
        await viewModel.SaveLayoutCommand.ExecuteAsync(null);

        //assert
        builder.MessageBoxService.Verify(
            x => x.ShowInfo(It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Once);
        Assert.False(viewModel.NotSavedChange);
    }
}
