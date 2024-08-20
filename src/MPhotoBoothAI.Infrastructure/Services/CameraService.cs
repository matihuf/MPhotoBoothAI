using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Infrastructure.Services;

public class CameraService(VideoCapture videoCapture) : ICameraService
{
    private readonly VideoCapture _videoCapture = videoCapture;
    private readonly List<IObserver> _observers = new();
    public bool IsOpened => _videoCapture != null && _videoCapture.IsOpened;

    public void Start()
    {
        _videoCapture.Start();
        _videoCapture.ImageGrabbed += CaptureDevice_ImageGrabbed;
    }

    private void CaptureDevice_ImageGrabbed(object? sender, EventArgs e)
    {
        var mat = _videoCapture.QueryFrame();
        Notify(mat);
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _videoCapture != null)
        {
            _videoCapture.ImageGrabbed -= CaptureDevice_ImageGrabbed;
            _videoCapture.Stop();
        }
    }
}
