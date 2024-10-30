
namespace MPhotoBoothAI.Application.Interfaces;
public interface IMessageBoxService
{
    Task<bool> ShowYesNo(string title, string text);
}
