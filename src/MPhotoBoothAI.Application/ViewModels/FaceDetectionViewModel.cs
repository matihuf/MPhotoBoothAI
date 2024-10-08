using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Extensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using SkiaSharp;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly IFaceSwapManager _faceSwapManager;
    private readonly IFilePickerService _filePickerService;
    private readonly ICameraDevice _cameraDevice;

    private readonly Mat _target;

    [ObservableProperty]
    private SKBitmap _frame;

    [RelayCommand]
    private void Swap()
    {
        _cameraDevice.Detach(this);
        Frame = _faceSwapManager.Swap(Frame.ToMat(), _target).ToSKBitmap();
    }

    [RelayCommand]
    private void Reset() => _cameraDevice.Attach(this);

    [RelayCommand]
    private async Task SetTarget()
    {
        byte[] target = await _filePickerService.PickFile();
        CvInvoke.Imdecode(target, ImreadModes.Color, _target);
    }

    public FaceDetectionViewModel(ICameraDevice cameraService, IFaceSwapManager faceSwapManager, IFilePickerService filePickerService)
    {
        _cameraDevice = cameraService;
        _cameraDevice.Start();
        _cameraDevice.Attach(this);
        _faceSwapManager = faceSwapManager;
        _filePickerService = filePickerService;
        _target = new Mat();
    }

    public void Notify(SKBitmap bitmap)
    {
        Frame = bitmap;
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
            _cameraDevice.Detach(this);
            Frame.Dispose();
            _target.Dispose();
        }
    }
}
