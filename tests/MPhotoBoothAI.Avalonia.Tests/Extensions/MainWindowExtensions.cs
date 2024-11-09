using Avalonia.Controls;
using MPhotoBoothAI.Avalonia.Views;
using MPhotoBoothAI.Models.UI;

namespace MPhotoBoothAI.Avalonia.Tests.Extensions;
public static class MainWindowExtensions
{
    public static void OpenView(this MainWindow window, Type viewModelType)
    {
        var listBoxPage = window.FindControl<ListBox>("ListBoxPage");
        for (int i = 0; i < listBoxPage.Items.Count; i++)
        {
            var expectedViewModel = (listBoxPage.Items[i] as ListItemTemplate).ModelType;
            if (expectedViewModel == viewModelType)
            {
                var listBoxItem = listBoxPage.ContainerFromIndex(i) as ListBoxItem;
                window.MouseClick(listBoxItem.Bounds.Center);
                return;
            }
        }
    }

    public static T FindViewControl<T>(this MainWindow window, string name) where T : Control
        => window.FindControl<ContentControl>("Content").FindControls<UserControl>().First().FindControl<T>(name);
}
