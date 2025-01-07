using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
using MPhotoBoothAI.Avalonia.Windows.FaceSwapTemplates;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.WindowParameters;
using MPhotoBoothAI.Models.WindowResults;

namespace MPhotoBoothAI.Avalonia.Tests.Windows;

public class AddFaceSwapTemplateWindowTests(DependencyInjectionFixture dependencyInjectionFixture) : BaseMainWindowTests(dependencyInjectionFixture)
{
    private readonly DependencyInjectionFixture _dependencyInjectionFixture = dependencyInjectionFixture;

    [AvaloniaFact]
    public async Task Close_MainWindowShouldNotHaveOwnedWindows()
    {
        //arrange
        var mainWindow = _builder.Build();
        var windowService = _dependencyInjectionFixture.ServiceProvider.GetRequiredService<IWindowService>();
        await windowService.Open<AddFaceSwapTemplateResults, AddFaceSwapTemplateParameters>(typeof(AddFaceSwapTemplateViewModel), mainWindow, new AddFaceSwapTemplateParameters(1),
            out IWindow window, false);
        var btnClose = ((AddFaceSwapTemplateWindow)window).FindControl<Button>("btnClose");
        //act
        Assert.NotEmpty(mainWindow.OwnedWindows);
        btnClose.Command.Execute(mainWindow);
        //arrange
        Assert.Empty(mainWindow.OwnedWindows);
    }
}
