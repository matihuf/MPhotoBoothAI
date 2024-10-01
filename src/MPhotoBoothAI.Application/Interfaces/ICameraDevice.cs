using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.Interfaces;

public interface ICameraDevice : ISubject
{
    void Start();
}
