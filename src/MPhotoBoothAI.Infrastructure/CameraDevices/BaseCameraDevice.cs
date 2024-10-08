using MPhotoBoothAI.Application.Interfaces.Observers;
using SkiaSharp;

namespace MPhotoBoothAI.Infrastructure.CameraDevices;

public abstract class BaseCameraDevice()
{
    private readonly List<IObserver> _observers = [];

    public void Attach(IObserver observer) => _observers.Add(observer);

    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify(SKBitmap bitmap)
    {
        foreach (var observer in _observers.ToList())
        {
            try
            {
                if (bitmap != null && !bitmap.IsEmpty)
                {
                    observer.Notify(bitmap.Copy());
                }
            }
            finally
            {
                bitmap?.Dispose();
            }
        }
    }
}
