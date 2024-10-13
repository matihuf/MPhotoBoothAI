using Avalonia;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace MPhotoBoothAI.Avalonia;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Default", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.File(configuration["Serilog:FilePath"], fileSizeLimitBytes: 2000000, rollOnFileSizeLimit: true)
                .CreateLogger();
            Log.Information("Application started, Version {version}", typeof(Program)?.Assembly?.GetName()?.Version?.ToString());

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
