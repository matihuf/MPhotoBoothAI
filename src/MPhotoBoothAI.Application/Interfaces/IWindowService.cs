namespace MPhotoBoothAI.Application.Interfaces;
public interface IWindowService
{
    public void Open<T>(Type viewModel, IMainWindow mainWindow, T parameters) where T : class;
}
