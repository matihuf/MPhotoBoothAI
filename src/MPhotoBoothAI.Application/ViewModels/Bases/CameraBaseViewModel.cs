using CommunityToolkit.Mvvm.ComponentModel;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels.Bases;
public partial class CameraBaseViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly ICameraDevice? _cameraDevice;

    [ObservableProperty]
    private Mat? _cameraFrame;

    public CameraBaseViewModel(ICameraManager cameraManager)
    {
        _cameraDevice = cameraManager.Current;
        _cameraDevice?.Attach(this);
    }

    public void Notify(Mat mat) => CameraFrame = mat;

    protected void ClearCameraFrame()
    {
        if (CameraFrame != null)
        {
            CameraFrame.Dispose();
            CameraFrame = null;
        }
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
            _cameraDevice?.Detach(this);
            ClearCameraFrame();
        }
    }
}
