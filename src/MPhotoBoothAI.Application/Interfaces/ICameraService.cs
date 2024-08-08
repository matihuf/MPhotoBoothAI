using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.Interfaces;

public interface ICameraService : ISubject, IDisposable
{
    void Start();
}
