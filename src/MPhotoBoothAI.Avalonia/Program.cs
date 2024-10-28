using Avalonia;
using MPhotoBoothAI.Infrastructure.Services;
using Serilog;
using System;

namespace MPhotoBoothAI.Avalonia;

sealed class Program
{
    private Program() { }

    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            var applicationInfoService = new ApplicationInfoService();
#if !DEBUG
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Default", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.File(Path.Combine(applicationInfoService.UserProfilePath, "Logs", "log.txt"), fileSizeLimitBytes: 2000000, rollOnFileSizeLimit: true)
                .CreateLogger();
            Log.Information("Application started, Version {version}", applicationInfoService.Version);
#endif

            BuildAvaloniaApp()
               .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
#if !DEBUG
            Log.Fatal(e, "Something very bad happened");
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
