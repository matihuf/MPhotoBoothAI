using CommunityToolkit.Mvvm.ComponentModel;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly IFaceDetectionService _yoloFaceService;
    private readonly ICameraService _cameraService;

    [ObservableProperty]
    private Mat _frame;

    public FaceDetectionViewModel(ICameraService cameraService, IFaceDetectionService yoloFaceService)
    {
        _cameraService = cameraService;
        _cameraService.Start();
        _cameraService.Attach(this);
        _yoloFaceService = yoloFaceService;
    }

    public void Notify(Mat mat)
    {
        var faces = _yoloFaceService.Detect(mat, 0.45f, 0.5f);
        Frame = mat.Clone();
        mat.Dispose();
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
            _cameraService.Detach(this);
            Frame.Dispose();
        }
    }
}
