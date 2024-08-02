using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia .Views;
using AvaloniaApplication = Avalonia.Application;
namespace MPhotoBoothAI.Avalonia;

public partial class App : AvaloniaApplication
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.Configure();
        ServiceProvider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<FaceDetectionViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}