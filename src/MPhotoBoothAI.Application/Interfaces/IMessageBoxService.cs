
namespace MPhotoBoothAI.Application.Interfaces;
public interface IMessageBoxService
{
    Task<bool> ShowYesNo(string title, string text, IWindow mainWindow);
    Task<string> ShowInput(string title, string text, int maxLength, IWindow mainWindow);
    Task<bool> ShowInfo(string title, string text, IWindow mainWindow);
}
