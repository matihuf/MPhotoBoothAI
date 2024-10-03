using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Infrastructure.Services;

public class NavigationService(IHistoryRouter<ViewModelBase> router) : INavigationService
{
    public T GoTo<T>() where T : ViewModelBase => router.GoTo<T>();
    public void GoTo(Type type) => router.GoTo(type);
    public void Forward() => router.Forward();
    public void Back() => router.Back();
}
