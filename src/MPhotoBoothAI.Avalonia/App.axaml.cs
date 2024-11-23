using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Views;
using MPhotoBoothAI.Models.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using AvaloniaApplication = Avalonia.Application;
namespace MPhotoBoothAI.Avalonia;

public partial class App : AvaloniaApplication
{
    private static IServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _serviceProvider = ConfigureServiceProvider();
            _serviceProvider.GetRequiredService<IDatabaseContext>().Database.Migrate();
            SetApplicationLanguage(_serviceProvider.GetRequiredService<IDatabaseContext>());
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
            };
            desktop.Exit += Desktop_Exit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ServiceProvider ConfigureServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure();
        return serviceCollection.BuildServiceProvider();
    }

    private static void SetApplicationLanguage(IDatabaseContext databaseContext)
    {
        var userSettingsEntity = databaseContext.UserSettings.FirstOrDefault();
        if (userSettingsEntity == null)
        {
            userSettingsEntity = new UserSettingsEntity { CultureInfoName = Thread.CurrentThread.CurrentUICulture.Name };
            databaseContext.UserSettings.Add(userSettingsEntity);
            databaseContext.SaveChangesAsync();
        }
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(userSettingsEntity.CultureInfoName);
    }

    private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit -= Desktop_Exit;
        }
        (_serviceProvider as IDisposable)?.Dispose();
    }
}