using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Integration.Tests;

public class DependencyInjectionFixture : IDisposable
{
    private readonly TestDependencyInjection _testDependencyInjection;

    public ServiceProvider ServiceProvider => _testDependencyInjection.ServiceProvider;

    public DependencyInjectionFixture()
    {
        _testDependencyInjection = new TestDependencyInjection();
        _testDependencyInjection.Configure();
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
            _testDependencyInjection.Dispose();
        }
    }
}
