using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly IFaceMultiSwapManager _faceMultiSwapManager;
    private readonly IFilePickerService _filePickerService;
    private readonly ICameraDevice _cameraDevice;

    private readonly Mat _target;

    [ObservableProperty]
    private Mat _frame;

    [RelayCommand]
    private void Swap()
    {
        _cameraDevice.Detach(this);
        Frame = _faceMultiSwapManager.Swap(Frame, _target);
    }

    [RelayCommand]
    private void Reset() => _cameraDevice.Attach(this);

    [RelayCommand]
    private async Task SetTarget()
    {
        byte[] target = await _filePickerService.PickFile();
        CvInvoke.Imdecode(target, ImreadModes.Color, _target);
    }

    public FaceDetectionViewModel(ICameraManager cameraManager, IFaceMultiSwapManager faceSwapManager, IFilePickerService filePickerService)
    {
        _cameraDevice = cameraManager.Current;
        _faceMultiSwapManager = faceSwapManager;
        _filePickerService = filePickerService;
        _target = new Mat();
        Frame = new Mat();
    }

    public void Notify(Mat mat)
    {
        Frame = mat;
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
            Frame.Dispose();
            _target.Dispose();
        }
    }
}
