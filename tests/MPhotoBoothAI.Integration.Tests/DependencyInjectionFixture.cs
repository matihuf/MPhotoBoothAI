using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests;

public class DependencyInjectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DependencyInjectionFixture()
    {
        var services = new ServiceCollection();
        services.Configure();
        var descriptor = new ServiceDescriptor(typeof(ICameraDevice), typeof(DummyCameraDevice), ServiceLifetime.Singleton);
        services.Replace(descriptor);
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
