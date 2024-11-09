
namespace MPhotoBoothAI.Application.Interfaces;
public interface IMessageBoxService
{
    Task<bool> ShowYesNo(string title, string text, IMainWindow mainWindow);
    Task<string> ShowInput(string title, string text, IMainWindow mainWindow);
}
