﻿using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Navigation;
using MPhotoBoothAI.Avalonia.Views;

namespace MPhotoBoothAI.Avalonia.Tests.Builders;
public class MainWindowBuilder(DependencyInjectionAvaloniaFixture dependencyInjectionFixture) : IDisposable
{
    private MainWindow _mainWindow;
    private readonly DependencyInjectionAvaloniaFixture _dependencyInjectionFixture = dependencyInjectionFixture;

    public MainWindow Build()
    {
        var vm = new MainViewModel(new HistoryRouter<ViewModelBase>(t => (ViewModelBase)_dependencyInjectionFixture.ServiceProvider.GetRequiredService(t)));
        _mainWindow = new MainWindow { DataContext = vm };
        _mainWindow.Show();
        return _mainWindow;
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
            _mainWindow?.Close();
        }
    }
}
