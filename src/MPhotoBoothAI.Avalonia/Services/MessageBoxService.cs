using Avalonia.Controls;
using MPhotoBoothAI.Application.Interfaces;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace MPhotoBoothAI.Avalonia.Services;
public class MessageBoxService : IMessageBoxService
{
    public async Task<bool> ShowYesNo(string title, string text)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
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
            ButtonDefinitions = ButtonEnum.YesNo
        });
        var result = await box.ShowAsync();
        return result == ButtonResult.Yes;
    }
}
