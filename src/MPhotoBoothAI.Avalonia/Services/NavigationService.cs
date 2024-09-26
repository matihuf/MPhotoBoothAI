using System;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Navigation;

namespace MPhotoBoothAI.Avalonia.Services;

public class NavigationService(HistoryRouter<ViewModelBase> router)  : INavigationService
{
    private readonly HistoryRouter<ViewModelBase> _router = router;

    public T GoTo<T>() where T : ViewModelBase => _router.GoTo<T>();
    public void GoTo(Type type) => _router.GoTo(type);
    public void Forward() => _router.Forward();
    public void Back() => _router.Back();
}
