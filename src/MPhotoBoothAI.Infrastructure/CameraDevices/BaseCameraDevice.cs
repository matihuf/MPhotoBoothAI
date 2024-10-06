using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Infrastructure.CameraDevices;

public abstract class BaseCameraDevice()
{
    private readonly List<IObserver> _observers = [];

    public void Attach(IObserver observer) => _observers.Add(observer);

    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify(Mat mat)
    {
        foreach (var observer in _observers.ToList())
        {
            try
            {
                if (mat != null && !mat.IsEmpty)
                {
                    observer.Notify(mat.Clone());
                }
            }
            finally
            {
                mat?.Dispose();
            }
        }
    }
}
