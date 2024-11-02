using Avalonia;
using MPhotoBoothAI.Infrastructure.Services;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace MPhotoBoothAI.Avalonia;

sealed class Program
{
    private Program() { }

    [STAThread]
    public static void Main(string[] args)
    {
        var applicationInfoService = new ApplicationInfoService();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Default", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.File(Path.Combine(applicationInfoService.UserProfilePath, "Logs", "log.txt"), fileSizeLimitBytes: 2000000, rollOnFileSizeLimit: true)
            .CreateLogger();
        Log.Information("Application started, Version {version}", applicationInfoService.Version);

        BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
