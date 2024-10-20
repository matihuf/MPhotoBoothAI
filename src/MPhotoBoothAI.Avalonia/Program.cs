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

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
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
        catch (Exception e)
        {
            Log.Fatal(e, "Something very bad happened");
#if DEBUG
            throw;
#endif
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
