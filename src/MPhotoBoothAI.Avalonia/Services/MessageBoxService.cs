using Avalonia.Controls;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Threading.Tasks;

namespace MPhotoBoothAI.Avalonia.Services;
public class MessageBoxService : IMessageBoxService
{
    public async Task<bool> ShowYesNo(string title, string text, IMainWindow? mainWindow = null)
    {
        var box = BuildMessageBox(title, text, null);
        string result;
        if (mainWindow != null)
        {
            result = await box.ShowWindowDialogAsync((MainWindow)mainWindow);
        }
        else
        {
            result = await box.ShowAsync();
        }
        return result == Application.Assets.UI.yes;
    }

    public async Task<string> ShowInput(string title, string text, IMainWindow? mainWindow = null)
    {
        var box = BuildMessageBox(title, text, new InputParams { Label = string.Empty });
        string result;
        if (mainWindow != null)
        {
            result = await box.ShowWindowDialogAsync((MainWindow)mainWindow);
        }
        else
        {
            result = await box.ShowAsync();
        }
        return result == Application.Assets.UI.yes ? box.InputValue : string.Empty;
    }

    private static IMsBox<string> BuildMessageBox(string title, string text, InputParams? inputParams)
    {
        return MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentHeader = title,
            ContentMessage = text,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            CanResize = false,
            Width = 500,
            ShowInCenter = true,
            Topmost = true,
            SystemDecorations = SystemDecorations.BorderOnly,
            ButtonDefinitions =
            [
                new ButtonDefinition { Name = Application.Assets.UI.yes, IsCancel = false},
                new ButtonDefinition { Name = Application.Assets.UI.cancel, IsCancel = true },
            ],
            InputParams = inputParams
        });
    }
}
