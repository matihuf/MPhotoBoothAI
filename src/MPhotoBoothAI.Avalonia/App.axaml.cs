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
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ServiceProvider = ConfigureServiceProvider();
            ServiceProvider.GetRequiredService<IDatabaseContext>().Database.Migrate();
            SetApplicationLanguage(ServiceProvider.GetRequiredService<IDatabaseContext>());
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
            };
            desktop.Exit += Desktop_Exit;
        }

        base.OnFrameworkInitializationCompleted();
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
        (ServiceProvider as IDisposable)?.Dispose();
    }
}