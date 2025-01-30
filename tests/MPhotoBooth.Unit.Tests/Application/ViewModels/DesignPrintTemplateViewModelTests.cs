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
}
