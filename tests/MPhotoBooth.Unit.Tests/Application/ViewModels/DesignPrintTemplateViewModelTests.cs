using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels;
public class DesignPrintTemplateViewModelTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        var viewModel = new DesignPrintTemplateViewModelBuilder().Build();

        Assert.Equal(1, viewModel.Id);
        Assert.NotNull(viewModel.LayoutFormat);
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
