using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Avalonia;

namespace MPhotoBoothAI.Integration.Tests;

public class DependencyInjectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DependencyInjectionFixture()
    {
        var services = new ServiceCollection();
        services.Configure();
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServiceProvider.Dispose();
        }
    }
}
