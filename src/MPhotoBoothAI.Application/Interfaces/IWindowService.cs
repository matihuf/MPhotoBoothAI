namespace MPhotoBoothAI.Application.Interfaces;
public interface IWindowService
{
    public Task<Y?> Open<Y, T>(Type viewModel, IWindow mainWindow, T parameters, out IWindow? openedWindow, bool wait = true) where T : class where Y : class;
}
