using Avalonia.Controls;
using Avalonia.Themes.Neumorphism.Dialogs;
using Avalonia.Themes.Neumorphism.Dialogs.Enums;
using MPhotoBoothAI.Application.Interfaces;
using System.Threading.Tasks;

namespace MPhotoBoothAI.Avalonia.Services;
public class MessageBoxService : IMessageBoxService
{
    public async Task<bool> ShowInfo(string title, string text, IWindow? mainWindow = null)
    {
        var dialog = DialogHelper.CreateCommonDialog(new CommonDialogBuilderParams()
        {
            ContentHeader = title,
            SupportingText = text,
            StartupLocation = WindowStartupLocation.CenterOwner,
            NegativeResult = new DialogResult(Application.Assets.UI.cancel),
            Borderless = true,
            Width = 480,
            RightDialogButtons =
            [
                new DialogButton
                {
                    Content = Application.Assets.UI.ok,
                    Result = Application.Assets.UI.ok,
                    DialogButtonStyle = new DialogButtonStyle(DialogButtonBackgroundColor.PrimaryColor, DialogButtonForegroundColor.White)
                }
            ]
        });
        DialogResult result;
        if (mainWindow != null)
        {
            result = await dialog.ShowDialog((Window)mainWindow);
        }
        else
        {
            result = await dialog.Show();
        }
        return result.GetResult == Application.Assets.UI.ok;
    }

    public async Task<bool> ShowYesNo(string title, string text, IWindow? mainWindow = null)
    {
        var dialog = DialogHelper.CreateCommonDialog(new CommonDialogBuilderParams()
        {
            ContentHeader = title,
            SupportingText = text,
            StartupLocation = WindowStartupLocation.CenterOwner,
            NegativeResult = new DialogResult(Application.Assets.UI.cancel),
            Borderless = true,
            Width = 480,
            LeftDialogButtons = [new DialogButton { Content = Application.Assets.UI.cancel, Result = Application.Assets.UI.cancel }],
            RightDialogButtons =
            [
                new DialogButton
                {
                    Content = Application.Assets.UI.yes,
                    Result = Application.Assets.UI.yes,
                    DialogButtonStyle = new DialogButtonStyle(DialogButtonBackgroundColor.PrimaryColor, DialogButtonForegroundColor.White)
                }
            ]
        });

        DialogResult result;
        if (mainWindow != null)
        {
            result = await dialog.ShowDialog((Window)mainWindow);
        }
        else
        {
            result = await dialog.Show();
        }
        return result.GetResult == Application.Assets.UI.yes;
    }

    public async Task<string> ShowInput(string title, string text, int maxLength, IWindow? mainWindow = null)
    {
        var dialog = DialogHelper.CreateTextFieldDialog(new TextFieldDialogBuilderParams()
        {
            ContentHeader = title,
            StartupLocation = WindowStartupLocation.CenterOwner,
            Borderless = true,
            Width = 400,
            TextFields = [new() { Label = text, MaxCountChars = maxLength }],
            RightDialogButtons =
            [
                new DialogButton
                {
                    Content = Application.Assets.UI.cancel,
                    Result = Application.Assets.UI.cancel,
                    IsNegative = true
                },
                new DialogButton
                {
                    Content = Application.Assets.UI.yes,
                    Result = Application.Assets.UI.yes,
                    IsPositive = true,
                    DialogButtonStyle = new DialogButtonStyle(DialogButtonBackgroundColor.PrimaryColor, DialogButtonForegroundColor.White)
                }
            ],
        });

        TextFieldDialogResult result;
        if (mainWindow != null)
        {
            result = await dialog.ShowDialog((Window)mainWindow);
        }
        else
        {
            result = await dialog.Show();
        }
        return result.GetResult == Application.Assets.UI.yes ? result.GetFieldsResult()[0].Text : string.Empty;
    }
}
