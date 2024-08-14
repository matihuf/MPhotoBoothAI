using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using MPhotoBoothAI.Application.Managers;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class FaceDetectionViewModel : ViewModelBase, IObserver, IDisposable
{
    private readonly FaceSwapManager _faceSwapManager;
    private readonly ICameraService _cameraService;

    [ObservableProperty]
    private Mat _frame;

    [RelayCommand]
    private void Swap()
    {
        _cameraService.Detach(this);
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Frame = _faceSwapManager.Swap(Frame, CvInvoke.Imread($"{directory}/woman2.jpg"));
    }

    [RelayCommand]
    private void Reset()
    {
        _cameraService.Attach(this);
    }

    public FaceDetectionViewModel(ICameraService cameraService, FaceSwapManager faceSwapManager)
    {
        _cameraService = cameraService;
        _cameraService.Start();
        _cameraService.Attach(this);
        _faceSwapManager = faceSwapManager;
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
        }
    }
}
