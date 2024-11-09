using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Tests.Extensions;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.UI;

namespace MPhotoBoothAI.Avalonia.Tests.Views;

public class MainWindowTests(DependencyInjectionFixture dependencyInjectionFixture) : BaseMainWindowTests(dependencyInjectionFixture)
{
    [AvaloniaFact]
    public void DefaultPage_Home()
    {
        //arrange
        var window = _builder.Build();
        //act
        var currentViewModel = GetContentControl(window).Content as HomeViewModel;
        //assert
        Assert.NotNull(currentViewModel);
    }

    [AvaloniaFact]
    public void MenuClickListBoxPageItem_ContentShouldBeAsExpected()
    {
        //arrange
        var window = _builder.Build();
        var listBoxPage = window.FindControl<ListBox>("ListBoxPage");
        for (int i = 0; i < listBoxPage.Items.Count; i++)
        {
            var expectedViewModel = (listBoxPage.Items[i] as ListItemTemplate).ModelType;
            var listBoxItem = listBoxPage.ContainerFromIndex(i) as ListBoxItem;
            window.MouseClick(listBoxItem.Bounds.Center);
            //act
            var contentType = GetContentControl(window).Content.GetType();
            //assert
            Assert.True(contentType == expectedViewModel, $"Expected {expectedViewModel} - Result {contentType}");
        }
    }

    private static ContentControl GetContentControl(Window window) => window.FindControl<ContentControl>("Content");
}
