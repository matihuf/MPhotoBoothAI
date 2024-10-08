using SkiaSharp;

namespace MPhotoBoothAI.Application.Interfaces.Observers;

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(SKBitmap bitmap);
}
