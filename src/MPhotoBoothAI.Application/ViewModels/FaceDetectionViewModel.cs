using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly IFaceSwapManager _faceSwapManager;
    private readonly IFilePickerService _filePickerService;
    private readonly IFaceAlignManager _faceAlignManager;
    private readonly IFaceGenderService _faceGenderService;
    private readonly ICameraDevice _cameraDevice;

    private readonly Mat _target;

    [ObservableProperty]
    private Mat _frame;

    [ObservableProperty]
    private string _gender;

    [RelayCommand]
    private void Swap()
    {
        _cameraDevice.Detach(this);
        Frame = _faceSwapManager.Swap(Frame, _target);
    }

    [RelayCommand]
    private void Reset() => _cameraDevice.Attach(this);

    [RelayCommand]
    private async Task SetTarget()
    {
        byte[] target = await _filePickerService.PickFile();
        CvInvoke.Imdecode(target, ImreadModes.Color, _target);
    }

    public FaceDetectionViewModel(ICameraDevice cameraDevice, IFaceSwapManager faceSwapManager, IFilePickerService filePickerService, IFaceAlignManager faceAlignManager,
        IFaceGenderService faceGenderService)
    {
        _cameraDevice = cameraDevice;
        _cameraDevice.StartLiveView();
        _cameraDevice.Attach(this);
        _faceSwapManager = faceSwapManager;
        _filePickerService = filePickerService;
        _faceAlignManager = faceAlignManager;
        _faceGenderService = faceGenderService;
        _target = new Mat();
    }

    public void Notify(Mat mat)
    {
        Frame = mat;
        using var align = _faceAlignManager.GetAlign(Frame);
        if (align != null)
        {
            Gender = _faceGenderService.Get(align.Align).ToString();
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
            _cameraDevice.Detach(this);
            Frame.Dispose();
            _target.Dispose();
        }
    }
}
