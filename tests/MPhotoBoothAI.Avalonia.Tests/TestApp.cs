using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Avalonia.Tests;

public class TestApp : App
{
    protected override IServiceProvider ConfigureServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure();
        var descriptor = new ServiceDescriptor(typeof(ICameraDevice), typeof(DummyCameraDevice), ServiceLifetime.Singleton);
        serviceCollection.Replace(descriptor);
        return serviceCollection.BuildServiceProvider();
    }
}
