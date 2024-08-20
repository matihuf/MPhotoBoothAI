using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly FaceSwapManager _faceSwapManager;
    private readonly IFilePickerService _filePickerService;
    private readonly ICameraService _cameraService;
    private readonly INavigationService _navigationService;

    private Mat _target;
    [ObservableProperty]
    private Mat _frame;

    [RelayCommand]
    private void Swap()
    {
        _cameraService.Detach(this);
        Frame = _faceSwapManager.Swap(Frame, _target);
    }

    [RelayCommand]
    private void Reset() => _cameraService.Attach(this);

    [RelayCommand]
    private void Back() => _navigationService.Back();

    [RelayCommand]
    private async Task SetTarget()
    {
        byte[] target = await _filePickerService.PickFile();
        CvInvoke.Imdecode(target, ImreadModes.Color, _target);
    }

    public FaceDetectionViewModel(ICameraService cameraService, FaceSwapManager faceSwapManager, IFilePickerService filePickerService, INavigationService navigationService)
    {
        _cameraService = cameraService;
        _cameraService.Start();
        _cameraService.Attach(this);
        _faceSwapManager = faceSwapManager;
        _filePickerService = filePickerService;
        _target = new Mat();
        _navigationService = navigationService;
    }

    public void Notify(Mat mat)
    {
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
            _target.Dispose();
        }
    }
}
