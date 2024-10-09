using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Avalonia.Tests;

public class TestApp : App
{
    protected override IServiceProvider ConfigureServiceProvider()
    {
        var testDependencyInjection = new TestDependencyInjection();
        testDependencyInjection.Configure();
        return testDependencyInjection.ServiceProvider;
    }
}
