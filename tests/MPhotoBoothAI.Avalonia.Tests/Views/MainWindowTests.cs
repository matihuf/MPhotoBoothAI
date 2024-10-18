using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Tests.Extensions;
using MPhotoBoothAI.Avalonia.Views;
using MPhotoBoothAI.Models.UI;

namespace MPhotoBoothAI.Avalonia.Tests.Views;

public class MainWindowTests
{
    [AvaloniaFact]
    public void MainWindow_DefaultPage_Home()
    {
        //arrange
        var window = new MainWindow { DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>() };
        window.Show();
        var content = window.FindControl<ContentControl>("Content");
        //act
        var currentViewModel = content.Content as HomeViewModel;
        //assert
        window.Close();
        Assert.NotNull(currentViewModel);
    }

    [AvaloniaFact]
    public void MainWindow_MenuClickListBoxPageItem_ContentShouldBeAsExpected()
    {
        //arrange
        var window = new MainWindow { DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>() };
        window.Show();
        var listBoxPage = window.FindControl<ListBox>("ListBoxPage");
        for (int i = 0; i < listBoxPage.Items.Count; i++)
        {
            var expectedViewModel = (listBoxPage.Items[i] as ListItemTemplate).ModelType;
            var listBoxItem = listBoxPage.ContainerFromIndex(i) as ListBoxItem;
            window.MouseClick(listBoxItem.Bounds.Center);
            //act
            var contentType = window.FindControl<ContentControl>("Content").Content.GetType();
            //assert
            Assert.True(contentType == expectedViewModel, $"Expected {expectedViewModel} - Result {contentType}");
        }
        window.Close();
    }
}
