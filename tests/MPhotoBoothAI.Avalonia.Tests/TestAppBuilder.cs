using Avalonia;
using Avalonia.Headless;
using MPhotoBoothAI.Avalonia.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
namespace MPhotoBoothAI.Avalonia.Tests;

public static class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}