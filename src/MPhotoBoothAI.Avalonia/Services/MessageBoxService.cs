using Avalonia.Controls;
using MPhotoBoothAI.Application.Interfaces;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Threading.Tasks;

namespace MPhotoBoothAI.Avalonia.Services;
public class MessageBoxService : IMessageBoxService
{
    public async Task<bool> ShowYesNo(string title, string text)
    {
        var box = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentHeader = title,
            ContentMessage = text,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            CanResize = false,
            Width = 500,
            Height = 150,
            ShowInCenter = true,
            Topmost = true,
            SystemDecorations = SystemDecorations.None,
            ButtonDefinitions =
            [
                new ButtonDefinition { Name = Application.Assets.UI.yes, IsCancel = false},
                new ButtonDefinition { Name = Application.Assets.UI.cancel, IsCancel = true },
            ],
        });
        var result = await box.ShowAsync();
        return result == Application.Assets.UI.yes;
    }

    public async Task<string> ShowInput(string title, string text)
    {
        var box = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentHeader = title,
            ContentMessage = text,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            CanResize = false,
            Width = 500,
            Height = 180,
            ShowInCenter = true,
            Topmost = true,
            SystemDecorations = SystemDecorations.None,
            ButtonDefinitions =
            [
                new ButtonDefinition { Name = Application.Assets.UI.yes, IsCancel = false},
                new ButtonDefinition { Name = Application.Assets.UI.cancel, IsCancel = true },
            ],
            InputParams = new InputParams { Label = string.Empty }
        });
        var result = await box.ShowAsync();
        return result == Application.Assets.UI.yes ? box.InputValue : string.Empty;
    }
}
