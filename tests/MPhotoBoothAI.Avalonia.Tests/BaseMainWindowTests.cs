using Avalonia.Controls;
using MPhotoBoothAI.Avalonia.Tests.Builders;
using MPhotoBoothAI.Avalonia.Tests.Extensions;
using MPhotoBoothAI.Common.Tests;

namespace MPhotoBoothAI.Avalonia.Tests;
public abstract class BaseMainWindowTests(DependencyInjectionFixture dependencyInjectionFixture) : IClassFixture<DependencyInjectionFixture>, IDisposable
{
    protected readonly MainWindowBuilder _builder = new(dependencyInjectionFixture);

    protected static Button GetMessageBoxButtonNo(Window messageBoxWindow) => messageBoxWindow.FindControls<Button>().ElementAt(9);
    protected static Button GetMessageBoxButtonYes(Window messageBoxWindow) => messageBoxWindow.FindControls<Button>().ElementAt(8);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _builder.Dispose();
        }
    }
}