using MPhotoBoothAI.Models;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IUserSettingsService : IDisposable
{
    UserSettings Value { get; }
}
