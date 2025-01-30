using Avalonia.Controls;
using Avalonia.Themes.Neumorphism.Dialogs;
using Avalonia.Themes.Neumorphism.Dialogs.Enums;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Collections.Generic;
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

    public async Task<bool> ShowInfo(string title, string text, IMainWindow? mainWindow = null)
    {
        var box = BuildMessageBox(title, text, null, false);
        string result;
        if (mainWindow != null)
        {
            result = await box.ShowWindowDialogAsync((MainWindow)mainWindow);
        }
        else
        {
            result = await box.ShowAsync();
        }
        return result == Application.Assets.UI.close;
    }

    private static IMsBox<string> BuildMessageBox(string title, string text, InputParams? inputParams, bool showCancel = true)
    {
        List<ButtonDefinition> _buttonDefinition = new List<ButtonDefinition> {
                new ButtonDefinition { Name = showCancel ? Application.Assets.UI.yes : Application.Assets.UI.close, IsCancel = false},
        };
        if (showCancel)
        {
            _buttonDefinition.Add(new ButtonDefinition { Name = Application.Assets.UI.cancel, IsCancel = true });
        }
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
            ButtonDefinitions = _buttonDefinition,
            InputParams = inputParams
        });
    public async Task<bool> ShowYesNo(string title, string text, IWindow mainWindow)
    {
        var result = await DialogHelper.CreateCommonDialog(new CommonDialogBuilderParams()
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
        }).ShowDialog((Window)mainWindow);
        return result.GetResult == Application.Assets.UI.yes;
    }

    public async Task<string> ShowInput(string title, string text, int maxLength, IWindow mainWindow)
    {
        var result = await DialogHelper.CreateTextFieldDialog(new TextFieldDialogBuilderParams()
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
        }).ShowDialog((Window)mainWindow);
        return result.GetResult == Application.Assets.UI.yes ? result.GetFieldsResult()[0].Text : string.Empty;
    }
}
