using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace MPhotoBoothAI.Avalonia.Services;
public class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public Task<Y?> Open<Y, T>(Type viewModel, IMainWindow mainWindow, T parameters) where T : class where Y : class
    {
        if (viewModel is null)
        {
            return Task.FromResult<Y?>(null);
        }
        var tcs = new TaskCompletionSource<Y?>();
        var name = viewModel.FullName!.Replace("Application", "Avalonia", StringComparison.Ordinal).Replace("ViewModel", "Window", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null && _serviceProvider != null)
        {
            var window = (Window)Activator.CreateInstance(type)!;
            var service = _serviceProvider.GetRequiredService(viewModel);
            if (service is IWindowParam<T> serviceParam)
            {
                serviceParam.Parameters = parameters;
            }
            window.DataContext = service;
            mainWindow.IsEnabled = false;
            EventHandler<WindowClosingEventArgs>? handler = null;
            handler = (object? sender, WindowClosingEventArgs e) =>
            {
                try
                {
                    tcs.TrySetResult((service as IWindowResult<Y>)?.Result);
                    (service as IDisposable)?.Dispose();
                    service = null;
                }
                finally
                {
                    mainWindow.IsEnabled = true;
                    window.Closing -= handler;
                }
            };
            window.Closing += handler;
            window.ShowDialog<Y>((Window)mainWindow);
        }
        return tcs.Task;
    }
}
