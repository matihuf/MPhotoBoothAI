﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia;

namespace MPhotoBoothAI.Common.Tests;

public class DependencyInjectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public virtual bool AddAiModels { get; set; } = true;

    public DependencyInjectionFixture()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var serviceCollection = new ServiceCollection();
        serviceCollection.Configure(configuration, AddAiModels);
        ReplaceService(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }


    public virtual void ReplaceService(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton(s => new Mock<ICameraDevice>().Object));
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
