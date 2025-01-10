using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using Emgu.CV;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
using MPhotoBoothAI.Avalonia.Windows.FaceSwapTemplates;
using MPhotoBoothAI.Models.WindowParameters;

namespace MPhotoBoothAI.Avalonia.Tests.Windows;

public class PreviewFaceSwapTemplateWindowTests(DependencyInjectionAvaloniaFixture dependencyInjectionFixture) : BaseMainWindowTests(dependencyInjectionFixture)
{
    private readonly DependencyInjectionAvaloniaFixture _dependencyInjectionFixture = dependencyInjectionFixture;

    [AvaloniaFact]
    public async Task Close_MainWindowShouldNotHaveOwnedWindows()
    {
        //arrange
        var mainWindow = _builder.Build();
        var windowService = _dependencyInjectionFixture.ServiceProvider.GetRequiredService<IWindowService>();
        var parameters = new PreviewFaceSwapTemplateParameters(1, 1, "TestData/11412.jpg", 3);
        await windowService.Open<object, PreviewFaceSwapTemplateParameters>(typeof(PreviewFaceSwapTemplateViewModel), mainWindow, parameters, out IWindow window, false);
        var btnClose = ((PreviewFaceSwapTemplateWindow)window).FindControl<Button>("btnClose");
        //act
        Assert.NotEmpty(mainWindow.OwnedWindows);
        btnClose.Command.Execute(mainWindow);
        //arrange
        Assert.Empty(mainWindow.OwnedWindows);
    }

    [AvaloniaFact]
    public async Task OnOpen_DisplayImageFromParametersAsPreview()
    {
        //arrange
        var mainWindow = _builder.Build();
        var windowService = _dependencyInjectionFixture.ServiceProvider.GetRequiredService<IWindowService>();
        var parameters = new PreviewFaceSwapTemplateParameters(1, 1, "TestData/11412.jpg", 3);
        await windowService.Open<object, PreviewFaceSwapTemplateParameters>(typeof(PreviewFaceSwapTemplateViewModel), mainWindow, parameters, out IWindow window, false);
        //act
        var imgPreview = ((PreviewFaceSwapTemplateWindow)window).FindControl<Image>("imgPreview");
        //assert
        var writeableBitmap = (WriteableBitmap)imgPreview.Source;
        using var mat = CvInvoke.Imread(parameters.FilePath);
        Assert.Equal(mat.Height, writeableBitmap.Size.Height);
        Assert.Equal(mat.Width, writeableBitmap.Size.Width);
    }

    [AvaloniaFact]
    public async Task OnOpen_DisplayFacesCountFromParameters()
    {
        //arrange
        var mainWindow = _builder.Build();
        var windowService = _dependencyInjectionFixture.ServiceProvider.GetRequiredService<IWindowService>();
        var parameters = new PreviewFaceSwapTemplateParameters(1, 1, "TestData/11412.jpg", 3);
        await windowService.Open<object, PreviewFaceSwapTemplateParameters>(typeof(PreviewFaceSwapTemplateViewModel), mainWindow, parameters, out IWindow window, false);
        //act
        var tbFaces = ((PreviewFaceSwapTemplateWindow)window).FindControl<TextBlock>("tbFaces");
        //assert
        Assert.Equal(parameters.Faces.ToString(), tbFaces.Text);
    }
}
