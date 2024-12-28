namespace MPhotoBoothAI.Application.Interfaces;
public interface IWindowService
{
    public Task<Y?> Open<Y, T>(Type viewModel, IMainWindow mainWindow, T parameters) where T : class where Y : class;
}
