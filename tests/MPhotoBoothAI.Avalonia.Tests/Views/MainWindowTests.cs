using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.ViewModels;
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
    public void MainWindow_MenuClickRandomListBoxItem_ContentShouldBeAsExpected()
    {
        //arrange
        var window = new MainWindow { DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>() };
        window.Show();
        var listBoxPage = window.FindControl<ListBox>("ListBoxPage");
        var random = new Random();
        int randomIndex = random.Next(0, listBoxPage.Items.Count);
        var expectedViewModel = (listBoxPage.Items[randomIndex] as ListItemTemplate).ModelType;
        var listBoxItem = listBoxPage.ContainerFromIndex(randomIndex) as ListBoxItem;
        window.MouseDown(listBoxItem.Bounds.Center, MouseButton.Left);
        //act
        var content = window.FindControl<ContentControl>("Content");
        //assert
        window.Close();
        Assert.True(content.Content.GetType() == expectedViewModel);
    }
}
