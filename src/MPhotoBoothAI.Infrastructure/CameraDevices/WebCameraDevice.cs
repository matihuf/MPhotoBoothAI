using Emgu.CV;
using MPhotoBoothAI.Application.Enums;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.CameraDevices;

public class WebCameraDevice : BaseCameraDevice, ICameraDevice
{
    private readonly VideoCapture _videoCapture;
    private bool _started = false;

    public event EventHandler Connected;
    public event EventHandler Disconnected;

    public ECameraType CameraType => ECameraType.Usb;
    public bool IsAvailable { get; private set; } = false;

    public WebCameraDevice()
    {
        _videoCapture = new VideoCapture(0, VideoCapture.API.DShow);
        _videoCapture.ImageGrabbed += CaptureDevice_ImageGrabbed;
        IsAvailable = _videoCapture.IsOpened;
    }

    public void StartLiveView()
    {
        if (!_started)
        {
            _started = true;
            _videoCapture.Start();
        }
    }

    private void CaptureDevice_ImageGrabbed(object? sender, EventArgs e)
    {
        var mat = _videoCapture.QueryFrame();
        Notify(mat);
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

    public void StopLiveView()
    {
        _videoCapture.Stop();
        _started = false;
    }

    public void TakePhoto(bool autoFocus = false)
    {
        StartLiveView();
        StopLiveView();
    }
}
