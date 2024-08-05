namespace MPhotoBoothAI.Application;

public interface ICameraService : ISubject, IDisposable
{
    void Start();
}
