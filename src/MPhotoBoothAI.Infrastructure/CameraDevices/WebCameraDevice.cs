using Emgu.CV;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.CameraDevices;

public class WebCameraDevice : BaseCameraDevice, ICameraDevice, IDisposable
{
    private readonly VideoCapture _videoCapture;
    private bool _started = false;

    public WebCameraDevice()
    {
        _videoCapture = new VideoCapture(0);
    }

    public void Start()
    {
        if (!_started)
        {
            _started = true;
            _videoCapture.Start();
            _videoCapture.ImageGrabbed += CaptureDevice_ImageGrabbed;
        }
    }

    private void CaptureDevice_ImageGrabbed(object? sender, EventArgs e)
    {
        var bitmap = _videoCapture.QueryFrame().ToSKBitmap();
        Notify(bitmap);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _videoCapture.ImageGrabbed -= CaptureDevice_ImageGrabbed;
            _videoCapture.Stop();
            _videoCapture.Dispose();
        }
    }
}
