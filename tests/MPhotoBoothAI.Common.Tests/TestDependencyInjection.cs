using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia;

namespace MPhotoBoothAI.Common.Tests;

public class TestDependencyInjection : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public void Configure()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure();
        var cameraDescriptor = new ServiceDescriptor(typeof(ICameraDevice), new Mock<ICameraDevice>().Object.GetType(), ServiceLifetime.Singleton);
        serviceCollection.Replace(cameraDescriptor);
        ServiceProvider = serviceCollection.BuildServiceProvider();
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