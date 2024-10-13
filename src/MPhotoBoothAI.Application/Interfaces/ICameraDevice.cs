using MPhotoBoothAI.Application.Enums;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.Interfaces;

public interface ICameraDevice : ISubject, IDisposable
{
    ECameraType CameraType { get; }
    bool IsAvailable { get; }

    event EventHandler Connected;
    event EventHandler Disconnected;

    void StartLiveView();
    void StopLiveView();
    void TakePhoto(bool autoFocus = false);
}
