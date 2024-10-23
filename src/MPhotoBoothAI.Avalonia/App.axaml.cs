using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Views;
using System;
using System.Globalization;
using System.Threading;
using AvaloniaApplication = Avalonia.Application;
namespace MPhotoBoothAI.Avalonia;

public partial class App : AvaloniaApplication
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected virtual IServiceProvider ConfigureServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure();
        return serviceCollection.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceProvider = ConfigureServiceProvider();
        ServiceProvider.GetRequiredService<IDatabaseContext>().Migrate();
        SetApplicationLanguage(ServiceProvider.GetRequiredService<IUserSettingsService>());
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
            };
            desktop.Exit += Desktop_Exit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void SetApplicationLanguage(IUserSettingsService userSettings)
    {
        if (string.IsNullOrEmpty(userSettings.Value.CultureInfoName))
        {
            userSettings.Value.CultureInfoName = Thread.CurrentThread.CurrentUICulture.Name;
        }
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(userSettings.Value.CultureInfoName);
    }

    private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit -= Desktop_Exit;
        }
        (ServiceProvider as IDisposable)?.Dispose();
    }
}