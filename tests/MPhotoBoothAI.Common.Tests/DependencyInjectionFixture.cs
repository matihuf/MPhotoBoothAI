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

    public virtual string Configuration { get; set; } = "integration";

    public DependencyInjectionFixture()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{Configuration}.json").Build();
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure(configuration);
        ReplaceService(serviceCollection);
        serviceCollection.AddSingleton(s => new Mock<ICameraDevice>());
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }


    public virtual void ReplaceService(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient(s => new DatabaseContextBuilder().Build()));
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
