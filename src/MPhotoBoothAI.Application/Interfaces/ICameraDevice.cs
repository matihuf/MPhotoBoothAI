using MPhotoBoothAI.Application.Enums;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.Interfaces;

public interface ICameraDevice : ISubject, IDisposable
{
    ECameraType CameraType { get; }
    void StartLiveView();
    void StopLiveView();
    bool Connect();
    Task TakePhotoAsync(bool autoFocus = false);
}
