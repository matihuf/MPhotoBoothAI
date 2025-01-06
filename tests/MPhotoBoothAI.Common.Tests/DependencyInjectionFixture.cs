using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia;

namespace MPhotoBoothAI.Common.Tests;

public class DependencyInjectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DependencyInjectionFixture()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure(configuration);
        serviceCollection.Replace(ServiceDescriptor.Singleton(s => new Mock<ICameraDevice>().Object));
        serviceCollection.Replace(ServiceDescriptor.Transient(s => new DatabaseContextBuilder().Build()));
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
