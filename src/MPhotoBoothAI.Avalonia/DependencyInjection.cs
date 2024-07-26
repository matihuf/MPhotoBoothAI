using Microsoft.Extensions.DependencyInjection;
using MPhotoBoothAI.ViewModels;

namespace MPhotoBoothAI.Avalonia;

public static class DependencyInjection
{
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        AddViewModels(services);
        return services;
    }

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
    }
}
